using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

public sealed record StatusServicoResultado(
    string CStat,
    string XMotivo,
    bool Operando,
    bool Paralisado,
    DateTime ConsultadoEm,
    string? RetornoXml);

/// <summary>
/// `NFeStatusServico4` — consulta status do serviço de uma UF/ambiente.
/// Usado pela `ContingenciaPolicy` para detectar paralisação e ativar SVRS.
/// Cache de 5 min reduz tráfego em consultas repetidas.
/// </summary>
public sealed class NFeStatusServicoService
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    private readonly SefazSoapClient _soap;
    private readonly ConcurrentDictionary<string, (StatusServicoResultado Result, DateTime Expira)> _cache = new();

    public NFeStatusServicoService(SefazSoapClient soap)
    {
        _soap = soap;
    }

    public async Task<StatusServicoResultado> ConsultarStatusServicoAsync(
        AmbienteFiscal ambiente,
        string uf,
        X509Certificate2 cert,
        bool ignorarCache = false,
        CancellationToken cancellationToken = default)
    {
        var key = $"{uf}|{ambiente}";

        if (!ignorarCache && _cache.TryGetValue(key, out var entry) && entry.Expira > DateTime.UtcNow)
            return entry.Result;

        var cUF = ResolveCUF(uf);
        var cons = new ConsStatServ
        {
            TpAmb = ambiente == AmbienteFiscal.Producao ? "1" : "2",
            CUF = cUF,
        };
        var payload = ServicoXmlSerializer.Serializar(cons);

        var soap = await _soap.EnviarAsync(uf, ambiente, SefazServico.StatusServico, payload, cert, cancellationToken);
        if (!soap.Sucesso || soap.ResultMsg is null)
        {
            return new StatusServicoResultado("0", soap.ErroMensagem ?? "Falha de transporte", false, false, DateTime.UtcNow, null);
        }

        var ret = ServicoXmlSerializer.Deserializar<RetConsStatServ>(soap.ResultMsg);
        var resultado = new StatusServicoResultado(
            CStat: ret.CStat,
            XMotivo: ret.XMotivo,
            Operando: ret.CStat == SefazResultadoCodigo.ServicoOperando107,
            Paralisado: SefazResultadoCodigo.IsParalisacao(ret.CStat),
            ConsultadoEm: DateTime.UtcNow,
            RetornoXml: soap.ResultMsg);

        _cache[key] = (resultado, DateTime.UtcNow.Add(CacheTtl));
        return resultado;
    }

    /// <summary>
    /// Mapeia UF (sigla) para código IBGE (cUF). 5 prioritárias + autorizadoras especiais.
    /// </summary>
    private static string ResolveCUF(string uf) => uf switch
    {
        "SP" => "35",
        "RJ" => "33",
        "MG" => "31",
        "RS" => "43",
        "PR" => "41",
        "SVRS" => "43",  // SVRS roda no RS
        "SVAN" => "91",  // Ambiente Nacional usa cUF=91 por convenção
        _ => throw new ArgumentException($"cUF não mapeado para UF '{uf}'. Adicionar em ResolveCUF.", nameof(uf)),
    };
}
