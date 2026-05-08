using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

public sealed record InutilizacaoResultado(
    bool Inutilizado,
    string CStat,
    string XMotivo,
    string? Protocolo,
    string? RetornoXml);

/// <summary>
/// `NFeInutilizacao4` — descarta uma faixa contígua de numeração não-usada.
/// Útil antes do encerramento mensal para "fechar" buracos legais.
/// </summary>
public sealed class NFeInutilizacaoService
{
    private readonly SefazSoapClient _soap;
    private readonly XmlSignerC14N _signer;

    public NFeInutilizacaoService(SefazSoapClient soap, XmlSignerC14N signer)
    {
        _soap = soap;
        _signer = signer;
    }

    public async Task<InutilizacaoResultado> InutilizarAsync(
        string cnpj,
        string uf,
        int ano,
        string mod,
        int serie,
        long nNFIni,
        long nNFFin,
        string xJust,
        AmbienteFiscal ambiente,
        X509Certificate2 cert,
        CancellationToken cancellationToken = default)
    {
        if (xJust.Length is < 15 or > 255)
            throw new ArgumentException("Justificativa deve ter entre 15 e 255 caracteres.", nameof(xJust));
        if (nNFFin < nNFIni)
            throw new ArgumentException("nNFFin deve ser ≥ nNFIni.", nameof(nNFFin));
        if (cnpj.Length != 14)
            throw new ArgumentException("CNPJ deve ter 14 dígitos.", nameof(cnpj));

        var cUF = ResolveCUF(uf);
        var anoStr = (ano % 100).ToString("00");
        var serieStr = serie.ToString().PadLeft(3, '0');
        var nNFIniStr = nNFIni.ToString().PadLeft(9, '0');
        var nNFFinStr = nNFFin.ToString().PadLeft(9, '0');

        var inut = new InutNFe
        {
            InfInut = new InfInut
            {
                Id = $"ID{cUF}{anoStr}{cnpj}{mod}{serieStr}{nNFIniStr}{nNFFinStr}",
                TpAmb = ambiente == AmbienteFiscal.Producao ? "1" : "2",
                CUF = cUF,
                Ano = anoStr,
                CNPJ = cnpj,
                Mod = mod,
                Serie = serieStr,
                NNFIni = nNFIniStr,
                NNFFin = nNFFinStr,
                XJust = xJust,
            },
        };

        var xml = ServicoXmlSerializer.Serializar(inut);
        var assinado = _signer.Sign(xml, inut.InfInut.Id, cert);

        var soap = await _soap.EnviarAsync(uf, ambiente, SefazServico.Inutilizacao, assinado, cert, cancellationToken);
        if (!soap.Sucesso || soap.ResultMsg is null)
            return new InutilizacaoResultado(false, "0", soap.ErroMensagem ?? "Falha de transporte", null, null);

        var ret = ServicoXmlSerializer.Deserializar<RetInutNFe>(soap.ResultMsg);
        var info = ret.InfInut;

        // cStat=102 indica inutilização homologada
        return new InutilizacaoResultado(
            Inutilizado: info.CStat == "102",
            CStat: info.CStat,
            XMotivo: info.XMotivo,
            Protocolo: info.NProt,
            RetornoXml: soap.ResultMsg);
    }

    private static string ResolveCUF(string uf) => uf switch
    {
        "SP" => "35", "RJ" => "33", "MG" => "31", "RS" => "43", "PR" => "41",
        "SVRS" => "43", "SVAN" => "91",
        _ => throw new ArgumentException($"cUF não mapeado para UF '{uf}'.", nameof(uf)),
    };
}
