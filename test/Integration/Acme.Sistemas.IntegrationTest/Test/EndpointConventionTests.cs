using System.Reflection;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Endurece a convenção blueprint Acme: cada rota HTTP registrada em runtime deve residir em pasta
/// com os 4 arquivos do padrão (Endpoint+Request+Response+Map). Itera <see cref="EndpointDataSource"/>
/// — captura também rotas registradas fora do contrato <c>IEndpoint</c> — em vez de reflectir tipos.
/// </summary>
public class EndpointConventionTests : IntegrationTestBase
{
    private static readonly HashSet<string> AllowList = new(StringComparer.Ordinal)
    {
        "/health",
    };

    private static readonly string ApiRoot = LocateApiRoot();

    public EndpointConventionTests(DockerEnvironment docker) : base(docker) { }

    [Trait("Solucao", "Api")]
    [Trait("Acao", "Convencoes")]
    [SkippableFact(DisplayName = "Dado as rotas registradas em runtime, quando enumera EndpointDataSource, então cada rota /api/v1 reside em pasta com Endpoint+Request+Response+Map")]
    public void TodaRota_TemEndpointRequestResponseMap()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        var dataSource = Factory.Services.GetRequiredService<EndpointDataSource>();

        var rotas = dataSource.Endpoints
            .OfType<RouteEndpoint>()
            .Where(e => !AllowList.Contains(e.RoutePattern.RawText ?? string.Empty))
            .ToList();

        rotas.Should().NotBeEmpty("o teste só faz sentido com rotas registradas");

        var faltando = new List<string>();
        foreach (var rota in rotas)
        {
            var pattern = rota.RoutePattern.RawText ?? "(sem pattern)";

            var declaringType = rota.Metadata.OfType<MethodInfo>()
                .Select(m => ResolveEndpointType(m.DeclaringType))
                .FirstOrDefault(t => t is not null);

            if (declaringType is null)
            {
                faltando.Add($"{pattern}: não foi possível resolver IEndpoint declarante via metadata (MethodInfo).");
                continue;
            }

            var endpointFile = LocateEndpointFile(declaringType);
            if (endpointFile is null)
            {
                faltando.Add($"{pattern} ({declaringType.FullName}): arquivo {declaringType.Name}.cs não encontrado em {ApiRoot}");
                continue;
            }

            var folder = Path.GetDirectoryName(endpointFile)!;
            var baseName = declaringType.Name.EndsWith("Endpoint", StringComparison.Ordinal)
                ? declaringType.Name[..^"Endpoint".Length]
                : declaringType.Name;

            foreach (var sibling in new[] { "Request", "Response", "Map" })
            {
                var expected = Path.Combine(folder, $"{baseName}{sibling}.cs");
                if (!File.Exists(expected))
                    faltando.Add($"{pattern} ({declaringType.Name}): faltando {baseName}{sibling}.cs em {folder}");
            }
        }

        faltando.Should().BeEmpty(string.Join("\n", faltando));
    }

    private static Type? ResolveEndpointType(Type? t)
    {
        // Lambdas em Minimal API geram closures aninhadas (`<>c__DisplayClass...`) cujo DeclaringType
        // é a classe IEndpoint. Subimos a cadeia até achar um tipo `*Endpoint` em
        // `Acme.Sistemas.Atena.Api.Endpoints`.
        while (t is not null)
        {
            if (t.Namespace is not null
                && t.Namespace.StartsWith("Acme.Sistemas.Atena.Api.Endpoints", StringComparison.Ordinal)
                && t.Name.EndsWith("Endpoint", StringComparison.Ordinal)
                && !t.Name.Contains('<', StringComparison.Ordinal))
            {
                return t;
            }
            t = t.DeclaringType;
        }
        return null;
    }

    private static string? LocateEndpointFile(Type endpointType)
    {
        var matches = Directory.GetFiles(ApiRoot, $"{endpointType.Name}.cs", SearchOption.AllDirectories);
        return matches.Length == 0 ? null : matches[0];
    }

    private static string LocateApiRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, "src")))
            dir = dir.Parent;
        if (dir is null)
            throw new InvalidOperationException("Não localizei a raiz do repo (procurei por pasta `src`).");
        return Path.Combine(dir.FullName, "src", "Api", "Acme.Sistemas.Atena.Api", "Endpoints");
    }
}
