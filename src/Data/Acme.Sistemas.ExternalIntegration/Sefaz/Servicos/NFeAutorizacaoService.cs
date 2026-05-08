using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;
using NFeXmlPoco = Acme.Sistemas.Domain.Entities.Fiscal.Xml.NFe;
using NFeNamespaces = Acme.Sistemas.Domain.Entities.Fiscal.Xml.NFeNamespaces;
using NFeXmlSerializer = Acme.Sistemas.Domain.Entities.Fiscal.Xml.NFeXmlSerializer;
using XsdValidator = Acme.Sistemas.ExternalIntegration.Sefaz.XsdValidator;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

/// <summary>
/// Resultado normalizado de uma chamada de autorização — abstrai o formato XML
/// para os handlers de domínio.
/// </summary>
public sealed record AutorizacaoResultado(
    bool Autorizado,
    string CStat,
    string XMotivo,
    string? Protocolo,
    string? ChaveAcesso,
    DateTime? DhAutorizacao,
    string? NRecibo,
    string? RetornoXml);

/// <summary>
/// Serviço NFeAutorizacao4 — autoriza NF-e síncrona ou assíncronamente.
/// </summary>
public sealed class NFeAutorizacaoService
{
    private readonly SefazSoapClient _soap;
    private readonly XsdValidator _xsd;
    private readonly XmlSignerC14N _signer;

    public NFeAutorizacaoService(SefazSoapClient soap, XsdValidator xsd, XmlSignerC14N signer)
    {
        _soap = soap;
        _xsd = xsd;
        _signer = signer;
    }

    /// <summary>
    /// Autorização síncrona (indSinc=1). Retorna o protocolo direto na resposta.
    /// Pré-condição: a NFe deve ter Id="NFe<chave>" e cStat=999 no caso de inválida local.
    /// </summary>
    public async Task<AutorizacaoResultado> AutorizarSyncAsync(
        NFeXmlPoco nfe,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        bool validarXsdLocalmente = true,
        CancellationToken cancellationToken = default)
    {
        // Validação XSD local opcional (se XSDs estiverem embarcados; senão pula com aviso silencioso).
        if (validarXsdLocalmente && XsdValidator.TemSchemasCarregados())
        {
            var xmlBruto = NFeXmlSerializer.SerializarNFe(nfe);
            var resultado = _xsd.Validar(xmlBruto);
            if (!resultado.Valido)
            {
                var erros = string.Join("; ", resultado.Erros.Select(e => e.Mensagem));
                return new AutorizacaoResultado(
                    Autorizado: false,
                    CStat: SefazResultadoCodigo.ErroLocalValidacao,
                    XMotivo: $"Validação XSD local falhou: {erros}",
                    null, null, null, null, null);
            }
        }

        // Assina a NFe in-place
        var xmlNFe = NFeXmlSerializer.SerializarNFe(nfe);
        var xmlAssinado = _signer.Sign(xmlNFe, nfe.InfNFe.Id, cert);

        // Empacota no enviNFe (lote de 1 com indSinc=1)
        // Reusa o XML assinado como string crua dentro do enviNFe.
        var enviNFeXml =
            $"""<?xml version="1.0" encoding="utf-8"?><enviNFe versao="4.00" xmlns="{NFeNamespaces.Portal}"><idLote>1</idLote><indSinc>1</indSinc>{StripDeclaration(xmlAssinado)}</enviNFe>""";

        var soapResult = await _soap.EnviarAsync(uf, ambiente, SefazServico.Autorizacao, enviNFeXml, cert, cancellationToken);

        if (!soapResult.Sucesso || soapResult.ResultMsg is null)
            return new AutorizacaoResultado(false, "0", soapResult.ErroMensagem ?? "Falha de transporte", null, null, null, null, null);

        return ParseRetorno(soapResult.ResultMsg);
    }

    /// <summary>
    /// Autorização assíncrona (indSinc=0). Retorna recibo (`nRec`); cliente faz polling depois.
    /// </summary>
    public async Task<AutorizacaoResultado> AutorizarAsyncAsync(
        NFeXmlPoco nfe,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        CancellationToken cancellationToken = default)
    {
        var xmlNFe = NFeXmlSerializer.SerializarNFe(nfe);
        var xmlAssinado = _signer.Sign(xmlNFe, nfe.InfNFe.Id, cert);

        var enviNFeXml =
            $"""<?xml version="1.0" encoding="utf-8"?><enviNFe versao="4.00" xmlns="{NFeNamespaces.Portal}"><idLote>1</idLote><indSinc>0</indSinc>{StripDeclaration(xmlAssinado)}</enviNFe>""";

        var soapResult = await _soap.EnviarAsync(uf, ambiente, SefazServico.Autorizacao, enviNFeXml, cert, cancellationToken);
        if (!soapResult.Sucesso || soapResult.ResultMsg is null)
            return new AutorizacaoResultado(false, "0", soapResult.ErroMensagem ?? "Falha de transporte", null, null, null, null, null);

        return ParseRetorno(soapResult.ResultMsg);
    }

    /// <summary>
    /// Parser unificado de `retEnviNFe` — extrai protocolo (síncrono) ou recibo (assíncrono).
    /// Visível para tests sem precisar de SOAP/cert.
    /// </summary>
    public static AutorizacaoResultado ParseRetorno(string retEnviNFeXml)
    {
        var ret = ServicoXmlSerializer.Deserializar<RetEnviNFe>(retEnviNFeXml);

        // Modo síncrono: protNFe presente
        var prot = ret.ProtNFe?.FirstOrDefault();
        if (prot is not null)
        {
            var info = prot.InfProt;
            return new AutorizacaoResultado(
                Autorizado: SefazResultadoCodigo.IsAutorizado(info.CStat),
                CStat: info.CStat,
                XMotivo: info.XMotivo,
                Protocolo: info.NProt,
                ChaveAcesso: info.ChNFe,
                DhAutorizacao: info.DhRecbto == default ? null : info.DhRecbto,
                NRecibo: null,
                RetornoXml: retEnviNFeXml);
        }

        // Modo assíncrono: infRec presente
        if (ret.InfRec is not null)
        {
            return new AutorizacaoResultado(
                Autorizado: false, // ainda não autorizado, apenas recebido
                CStat: ret.CStat,
                XMotivo: ret.XMotivo,
                Protocolo: null,
                ChaveAcesso: null,
                DhAutorizacao: null,
                NRecibo: ret.InfRec.NRec,
                RetornoXml: retEnviNFeXml);
        }

        // Erro de lote (e.g., cStat=225 assinatura inválida)
        return new AutorizacaoResultado(
            Autorizado: false,
            CStat: ret.CStat,
            XMotivo: ret.XMotivo,
            Protocolo: null,
            ChaveAcesso: null,
            DhAutorizacao: null,
            NRecibo: null,
            RetornoXml: retEnviNFeXml);
    }

    private static string StripDeclaration(string xml)
    {
        if (xml.StartsWith("<?xml", StringComparison.Ordinal))
        {
            var idx = xml.IndexOf("?>", StringComparison.Ordinal);
            if (idx > 0) return xml[(idx + 2)..].TrimStart();
        }
        return xml;
    }
}
