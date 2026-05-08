using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

public sealed record EventoResultado(
    bool Registrado,
    string CStat,
    string XMotivo,
    string? Protocolo,
    string? ChaveAcesso,
    string? TipoEvento,
    string? RetornoXml);

/// <summary>
/// `NFeRecepcaoEvento4` — cancelamento (110111) e Carta de Correção (110110).
/// </summary>
public sealed class NFeRecepcaoEventoService
{
    private readonly SefazSoapClient _soap;
    private readonly XmlSignerC14N _signer;

    public NFeRecepcaoEventoService(SefazSoapClient soap, XmlSignerC14N signer)
    {
        _soap = soap;
        _signer = signer;
    }

    /// <summary>
    /// Cancela uma NFe autorizada. cStat=135 indica "Evento registrado e vinculado a NFe".
    /// </summary>
    public Task<EventoResultado> CancelarAsync(
        string chaveNFe,
        string nProtAutorizacao,
        string xJust,
        string cnpjEmitente,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        CancellationToken cancellationToken = default)
    {
        if (xJust.Length is < 15 or > 255)
            throw new ArgumentException("Justificativa de cancelamento deve ter entre 15 e 255 caracteres.", nameof(xJust));

        var evento = MontarEvento(
            chave: chaveNFe,
            cnpj: cnpjEmitente,
            uf: uf,
            ambiente: ambiente,
            tpEvento: TipoEvento.Cancelamento,
            descEvento: "Cancelamento",
            preencherDetalhe: det =>
            {
                det.NProt = nProtAutorizacao;
                det.XJust = xJust;
            });

        return EnviarEventoAsync(evento, ambiente, uf, cert, cancellationToken);
    }

    /// <summary>
    /// Emite Carta de Correção Eletrônica (CC-e). xCorrecao entre 15 e 1000 chars.
    /// </summary>
    public Task<EventoResultado> EmitirCCeAsync(
        string chaveNFe,
        string xCorrecao,
        string cnpjEmitente,
        int sequenciaEvento,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        CancellationToken cancellationToken = default)
    {
        if (xCorrecao.Length is < 15 or > 1000)
            throw new ArgumentException("Texto de correção deve ter entre 15 e 1000 caracteres.", nameof(xCorrecao));
        if (sequenciaEvento is < 1 or > 20)
            throw new ArgumentException("nSeqEvento deve estar entre 1 e 20.", nameof(sequenciaEvento));

        var evento = MontarEvento(
            chave: chaveNFe,
            cnpj: cnpjEmitente,
            uf: uf,
            ambiente: ambiente,
            tpEvento: TipoEvento.CartaCorrecao,
            descEvento: "Carta de Correcao",
            preencherDetalhe: det =>
            {
                det.XCorrecao = xCorrecao;
                det.XCondUso =
                    "A Carta de Correcao e disciplinada pelo paragrafo 1o-A do art. 7o do Convenio S/N, " +
                    "de 15 de dezembro de 1970 e pode ser utilizada para regularizacao de erro ocorrido " +
                    "na emissao de documento fiscal, desde que o erro nao esteja relacionado com: " +
                    "I - as variaveis que determinam o valor do imposto tais como: base de calculo, " +
                    "aliquota, diferenca de preco, quantidade, valor da operacao ou da prestacao; " +
                    "II - a correcao de dados cadastrais que implique mudanca do remetente ou do destinatario; " +
                    "III - a data de emissao ou de saida.";
            },
            nSeqEvento: sequenciaEvento.ToString());

        return EnviarEventoAsync(evento, ambiente, uf, cert, cancellationToken);
    }

    private async Task<EventoResultado> EnviarEventoAsync(
        Evento evento,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        CancellationToken cancellationToken)
    {
        // Empacota o evento no envEvento (lote de 1)
        var envEvento = new EnvEvento { IdLote = "1", Evento = { evento } };
        var envXml = ServicoXmlSerializer.Serializar(envEvento);

        // O signer só assina elementos com Id; assinamos o infEvento dentro do evento
        // Reusamos XmlSignerC14N para isso
        envXml = _signer.Sign(envXml, evento.InfEvento.Id, cert);

        var soap = await _soap.EnviarAsync(uf, ambiente, SefazServico.RecepcaoEvento, envXml, cert, cancellationToken);
        if (!soap.Sucesso || soap.ResultMsg is null)
            return new EventoResultado(false, "0", soap.ErroMensagem ?? "Falha de transporte", null, evento.InfEvento.ChNFe, evento.InfEvento.TpEvento, null);

        var ret = ServicoXmlSerializer.Deserializar<RetEnvEvento>(soap.ResultMsg);
        var infResp = ret.RetEvento?.FirstOrDefault()?.InfEvento;

        return new EventoResultado(
            Registrado: SefazResultadoCodigo.IsAutorizado(infResp?.CStat ?? ret.CStat),
            CStat: infResp?.CStat ?? ret.CStat,
            XMotivo: infResp?.XMotivo ?? ret.XMotivo,
            Protocolo: infResp?.NProt,
            ChaveAcesso: infResp?.ChNFe ?? evento.InfEvento.ChNFe,
            TipoEvento: infResp?.TpEvento ?? evento.InfEvento.TpEvento,
            RetornoXml: soap.ResultMsg);
    }

    /// <summary>
    /// Monta um evento (sem assinar) com Id no formato `ID<tpEvento><chNFe><nSeqEvento>` (54 chars).
    /// Visível para tests.
    /// </summary>
    public static Evento MontarEvento(
        string chave, string cnpj, string uf, AmbienteFiscal ambiente,
        string tpEvento, string descEvento,
        Action<DetEvento> preencherDetalhe,
        string nSeqEvento = "1")
    {
        var id = $"ID{tpEvento}{chave}{nSeqEvento.PadLeft(2, '0')}";
        var evento = new Evento
        {
            InfEvento = new InfEvento
            {
                Id = id,
                COrgao = ResolveCOrgao(uf),
                TpAmb = ambiente == AmbienteFiscal.Producao ? "1" : "2",
                CNPJ = cnpj,
                ChNFe = chave,
                DhEvento = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                TpEvento = tpEvento,
                NSeqEvento = nSeqEvento,
                DetEvento = new DetEvento { DescEvento = descEvento },
            },
        };
        preencherDetalhe(evento.InfEvento.DetEvento);
        return evento;
    }

    private static string ResolveCOrgao(string uf) => uf switch
    {
        "SP" => "35", "RJ" => "33", "MG" => "31", "RS" => "43", "PR" => "41",
        "SVRS" => "43", "SVAN" => "91",
        _ => throw new ArgumentException($"cOrgao não mapeado para UF '{uf}'.", nameof(uf)),
    };
}
