using System.Collections.Concurrent;
using System.Text.Json;
using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

/// <summary>
/// Resolve URL SEFAZ a partir de (UF, ambiente, serviço). Carrega o catálogo embarcado
/// uma vez e mantém em memória. Permite override por configuração (ex.: tenant em ambiente
/// privado de testes).
/// </summary>
public sealed class SefazUrlCatalog
{
    private const string ResourceName = "Acme.Sistemas.ExternalIntegration.Sefaz.Urls.sefaz-urls.json";

    private static readonly Lazy<CatalogoData> _embedded = new(LoadEmbedded);
    private readonly ConcurrentDictionary<string, string> _overrides = new(StringComparer.Ordinal);

    /// <summary>
    /// Adiciona ou substitui uma URL para uma combinação específica.
    /// Útil pra apontar SP-homolog para um mock interno em testes integrados.
    /// </summary>
    public void DefinirOverride(string uf, AmbienteFiscal ambiente, SefazServico servico, string url)
    {
        _overrides[BuildKey(uf, ambiente, servico)] = url;
    }

    /// <summary>
    /// Resolve a URL. Override tem precedência sobre catálogo embarcado.
    /// </summary>
    /// <exception cref="KeyNotFoundException">UF/ambiente/serviço sem entrada no catálogo nem override.</exception>
    public string Resolver(string uf, AmbienteFiscal ambiente, SefazServico servico)
    {
        var key = BuildKey(uf, ambiente, servico);
        if (_overrides.TryGetValue(key, out var over)) return over;

        var data = _embedded.Value;
        if (!data.UFs.TryGetValue(uf, out var ambs))
            throw new KeyNotFoundException($"UF '{uf}' não está no catálogo SEFAZ. UFs disponíveis: {string.Join(", ", data.UFs.Keys)}.");

        var ambKey = ambiente == AmbienteFiscal.Producao ? "producao" : "homologacao";
        if (!ambs.TryGetValue(ambKey, out var servicos))
            throw new KeyNotFoundException($"Ambiente '{ambKey}' não disponível para UF '{uf}'.");

        var servicoKey = ServicoKey(servico);
        if (!servicos.TryGetValue(servicoKey, out var url))
            throw new KeyNotFoundException($"Serviço '{servicoKey}' não disponível para {uf}/{ambKey}.");

        return url;
    }

    /// <summary>
    /// Helpers de conveniência por serviço — evitam typos no enum.
    /// </summary>
    public string GetAutorizacao(string uf, AmbienteFiscal amb) => Resolver(uf, amb, SefazServico.Autorizacao);
    public string GetRetAutorizacao(string uf, AmbienteFiscal amb) => Resolver(uf, amb, SefazServico.RetAutorizacao);
    public string GetConsultaProtocolo(string uf, AmbienteFiscal amb) => Resolver(uf, amb, SefazServico.ConsultaProtocolo);
    public string GetStatusServico(string uf, AmbienteFiscal amb) => Resolver(uf, amb, SefazServico.StatusServico);
    public string GetRecepcaoEvento(string uf, AmbienteFiscal amb) => Resolver(uf, amb, SefazServico.RecepcaoEvento);
    public string GetInutilizacao(string uf, AmbienteFiscal amb) => Resolver(uf, amb, SefazServico.Inutilizacao);

    /// <summary>
    /// UFs cobertas pelo catálogo embarcado (não inclui a lista pendente).
    /// </summary>
    public IReadOnlyCollection<string> UFsDisponiveis => (IReadOnlyCollection<string>)_embedded.Value.UFs.Keys;

    private static string BuildKey(string uf, AmbienteFiscal amb, SefazServico s) =>
        $"{uf}|{amb}|{s}";

    private static string ServicoKey(SefazServico s) => s switch
    {
        SefazServico.Autorizacao => "autorizacao",
        SefazServico.RetAutorizacao => "retAutorizacao",
        SefazServico.ConsultaProtocolo => "consultaProtocolo",
        SefazServico.StatusServico => "statusServico",
        SefazServico.RecepcaoEvento => "recepcaoEvento",
        SefazServico.Inutilizacao => "inutilizacao",
        _ => throw new ArgumentOutOfRangeException(nameof(s), s, null),
    };

    private static CatalogoData LoadEmbedded()
    {
        var asm = typeof(SefazUrlCatalog).Assembly;
        using var stream = asm.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException(
                $"Recurso embarcado '{ResourceName}' não encontrado. Verifique se o sefaz-urls.json está como EmbeddedResource no .csproj.");

        var raw = JsonDocument.Parse(stream);
        var ufsNode = raw.RootElement.GetProperty("ufs");

        var ufs = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>(StringComparer.Ordinal);
        foreach (var ufProp in ufsNode.EnumerateObject())
        {
            var ambs = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
            foreach (var ambProp in ufProp.Value.EnumerateObject())
            {
                if (ambProp.Name.StartsWith('_')) continue; // _descricao, _comment etc.
                if (ambProp.Value.ValueKind != JsonValueKind.Object) continue;

                var servicos = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (var sProp in ambProp.Value.EnumerateObject())
                {
                    if (sProp.Value.ValueKind == JsonValueKind.String)
                        servicos[sProp.Name] = sProp.Value.GetString()!;
                }
                ambs[ambProp.Name] = servicos;
            }
            ufs[ufProp.Name] = ambs;
        }

        return new CatalogoData(ufs);
    }

    private sealed record CatalogoData(IReadOnlyDictionary<string, Dictionary<string, Dictionary<string, string>>> UFs);
}
