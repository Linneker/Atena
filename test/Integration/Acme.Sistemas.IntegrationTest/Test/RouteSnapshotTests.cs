using System.Text.Json;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Oracle de rotas — guarda das mudanças durante o split-endpoints-monolitos.
/// Enumera <see cref="EndpointDataSource"/> em runtime e compara com baseline JSON.
/// Qualquer typo de path/verb/name no split quebra esse teste.
/// </summary>
public class RouteSnapshotTests : IntegrationTestBase
{
    private static readonly string BaselinePath = Path.GetFullPath(Path.Combine(
        AppContext.BaseDirectory,
        "..", "..", "..", "..", "..", "..",
        "openspec", "changes", "split-endpoints-monolitos", "baseline", "routes-runtime.json"));

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };

    public RouteSnapshotTests(DockerEnvironment docker) : base(docker) { }

    [SkippableFact]
    public void RotasEnumeradas_BatemComBaseline()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        // CreateClient garante que o pipeline foi montado e endpoints estão registrados.
        _ = Factory.CreateClient();

        var dataSource = Factory.Services.GetRequiredService<EndpointDataSource>();

        var atual = SnapshotRotas(dataSource);
        var atualJson = JsonSerializer.Serialize(atual, JsonOptions);

        if (!File.Exists(BaselinePath))
        {
            // Primeira execução: gera baseline e falha com instruções.
            Directory.CreateDirectory(Path.GetDirectoryName(BaselinePath)!);
            File.WriteAllText(BaselinePath, atualJson);
            Assert.Fail(
                $"Baseline criado em {BaselinePath}. Revise o conteúdo e commite. Próximas execuções vão comparar contra este arquivo.");
        }

        var baselineJson = File.ReadAllText(BaselinePath);
        var baseline = JsonSerializer.Deserialize<List<RotaSnapshot>>(baselineJson)!;

        atual.Should().BeEquivalentTo(baseline,
            opts => opts.WithStrictOrdering(),
            because: $"qualquer divergência indica regressão de routing — para atualizar baseline, delete {BaselinePath} e re-rode o teste.");
    }

    private static List<RotaSnapshot> SnapshotRotas(EndpointDataSource dataSource)
    {
        return dataSource.Endpoints
            .OfType<RouteEndpoint>()
            .Select(e => new RotaSnapshot(
                Pattern: e.RoutePattern.RawText ?? string.Empty,
                Verbs: e.Metadata.OfType<Microsoft.AspNetCore.Routing.HttpMethodMetadata>()
                    .SelectMany(m => m.HttpMethods)
                    .OrderBy(v => v, StringComparer.Ordinal)
                    .ToArray(),
                DisplayName: e.DisplayName ?? string.Empty))
            .OrderBy(r => r.Pattern, StringComparer.Ordinal)
            .ThenBy(r => string.Join(",", r.Verbs), StringComparer.Ordinal)
            .ThenBy(r => r.DisplayName, StringComparer.Ordinal)
            .ToList();
    }

    public sealed record RotaSnapshot(string Pattern, string[] Verbs, string DisplayName);
}
