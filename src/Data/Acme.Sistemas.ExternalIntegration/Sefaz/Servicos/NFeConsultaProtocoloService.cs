using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

/// <summary>
/// `NFeConsultaProtocolo4` — consulta status atual de uma NFe pela chave de acesso.
/// Útil para reconciliação de NFes "perdidas" (autorizadas na SEFAZ sem retorno do lote).
/// </summary>
public sealed class NFeConsultaProtocoloService
{
    private readonly SefazSoapClient _soap;

    public NFeConsultaProtocoloService(SefazSoapClient soap)
    {
        _soap = soap;
    }

    public async Task<AutorizacaoResultado> ConsultarChaveAsync(
        string chave,
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        CancellationToken cancellationToken = default)
    {
        if (chave.Length != 44)
            throw new ArgumentException("Chave de acesso deve ter 44 dígitos.", nameof(chave));

        var cons = new ConsSitNFe
        {
            TpAmb = ambiente == AmbienteFiscal.Producao ? "1" : "2",
            ChNFe = chave,
        };
        var payload = ServicoXmlSerializer.Serializar(cons);

        var soap = await _soap.EnviarAsync(uf, ambiente, SefazServico.ConsultaProtocolo, payload, cert, cancellationToken);
        if (!soap.Sucesso || soap.ResultMsg is null)
            return new AutorizacaoResultado(false, "0", soap.ErroMensagem ?? "Falha de transporte", null, chave, null, null, null);

        var ret = ServicoXmlSerializer.Deserializar<RetConsSitNFe>(soap.ResultMsg);
        var prot = ret.ProtNFe;

        return new AutorizacaoResultado(
            Autorizado: SefazResultadoCodigo.IsAutorizado(prot?.InfProt.CStat ?? ret.CStat),
            CStat: prot?.InfProt.CStat ?? ret.CStat,
            XMotivo: prot?.InfProt.XMotivo ?? ret.XMotivo,
            Protocolo: prot?.InfProt.NProt,
            ChaveAcesso: ret.ChNFe ?? chave,
            DhAutorizacao: prot?.InfProt.DhRecbto == default ? null : prot?.InfProt.DhRecbto,
            NRecibo: null,
            RetornoXml: soap.ResultMsg);
    }
}
