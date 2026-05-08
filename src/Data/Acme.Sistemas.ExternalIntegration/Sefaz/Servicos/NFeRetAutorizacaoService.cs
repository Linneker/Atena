using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

/// <summary>
/// `NFeRetAutorizacao4` — consulta resultado de um lote enviado em modo assíncrono.
/// Faz polling até `cStat=104` (lote processado) ou esgota tentativas.
/// </summary>
public sealed class NFeRetAutorizacaoService
{
    private readonly SefazSoapClient _soap;

    public NFeRetAutorizacaoService(SefazSoapClient soap)
    {
        _soap = soap;
    }

    /// <summary>
    /// Faz polling de um recibo. Retorna assim que o lote sair de "em processamento" (cStat=105).
    /// </summary>
    /// <param name="maxTentativas">Default 6 (~30s com backoff inicial 2s)</param>
    public async Task<AutorizacaoResultado> ConsultarReciboAsync(
        string nRec,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        int maxTentativas = 6,
        CancellationToken cancellationToken = default)
    {
        var consulta = new ConsReciNFe
        {
            TpAmb = ambiente == AmbienteFiscal.Producao ? "1" : "2",
            NRec = nRec,
        };
        var payload = ServicoXmlSerializer.Serializar(consulta);

        var delay = TimeSpan.FromSeconds(2);
        for (var tentativa = 1; tentativa <= maxTentativas; tentativa++)
        {
            var soap = await _soap.EnviarAsync(uf, ambiente, SefazServico.RetAutorizacao, payload, cert, cancellationToken);
            if (!soap.Sucesso || soap.ResultMsg is null)
                return new AutorizacaoResultado(false, "0", soap.ErroMensagem ?? "Falha de transporte", null, null, null, nRec, null);

            var ret = ServicoXmlSerializer.Deserializar<RetConsReciNFe>(soap.ResultMsg);

            if (ret.CStat == SefazResultadoCodigo.LoteEmProcessamento105)
            {
                if (tentativa == maxTentativas) break;
                await Task.Delay(delay, cancellationToken);
                delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 30));
                continue;
            }

            // Lote processado (cStat=104) — protocolo de cada NFe vem em ret.ProtNFe
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
                    NRecibo: nRec,
                    RetornoXml: soap.ResultMsg);
            }

            // Erro de lote (e.g., cStat=225)
            return new AutorizacaoResultado(false, ret.CStat, ret.XMotivo, null, null, null, nRec, soap.ResultMsg);
        }

        return new AutorizacaoResultado(
            Autorizado: false,
            CStat: SefazResultadoCodigo.LoteEmProcessamento105,
            XMotivo: "Lote ainda em processamento após máximo de tentativas — tente consultar novamente mais tarde.",
            Protocolo: null,
            ChaveAcesso: null,
            DhAutorizacao: null,
            NRecibo: nRec,
            RetornoXml: null);
    }
}
