using System.Reflection;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Mediators.Notification;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Convenções do blueprint Acme (lado Services):
///
/// • Para cada `*Command` em <c>Acme.Sistemas.Services.V1</c>: exigir Handler/Behavior/Result/Validation
///   no mesmo namespace (mesma pasta).
/// • Idem para `*Query` e `*Notification` (Notification não tem Result).
///
/// O teste lê o **disco** (paths físicos) porque a convenção é de organização de arquivos, não só de tipos.
///
/// O analyzer de endpoints (Endpoint+Request+Response+Map) vive em
/// <c>EndpointConventionTests</c> no projeto de integração — itera <c>EndpointDataSource</c> em runtime
/// para cobrir também rotas registradas fora do contrato <c>IEndpoint</c>.
/// </summary>
public class ConvencoesBlueprintTests
{
    private static readonly string SrcRoot = LocateSrcRoot();

    private static string LocateSrcRoot()
    {
        // Test bin path: .../test/Unit/.../bin/Debug/net10.0
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, "src")))
        {
            dir = dir.Parent;
        }
        if (dir is null) throw new InvalidOperationException("Não localizei a raiz do repo (procurei por pasta `src`).");
        return Path.Combine(dir.FullName, "src");
    }

    private static IEnumerable<Type> RequestTypes(string suffix)
    {
        var asm = typeof(Acme.Sistemas.Services.ServicesServiceCollection).Assembly;
        return asm.GetTypes()
            .Where(t => !t.IsAbstract
                && !t.IsInterface
                && t.Name.EndsWith(suffix, StringComparison.Ordinal)
                && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
                && (t.Namespace?.StartsWith("Acme.Sistemas.Services.V1.") ?? false));
    }

    private static string FolderForType(Type t)
    {
        // Map namespace `Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa` to disk path
        var rel = t.Namespace!.Replace("Acme.Sistemas.Services", "Acme.Sistemas.Services");
        var parts = rel.Split('.');
        // src\Service\Acme.Sistemas.Services\V1\Despesa\Command\AlterarDespesa
        var path = Path.Combine(SrcRoot, "Service", parts[0] + "." + parts[1] + "." + parts[2]);
        for (var i = 3; i < parts.Length; i++)
        {
            path = Path.Combine(path, parts[i]);
        }
        return path;
    }

    [Trait("Solucao", "Test")]
    [Trait("Acao", "Convencoes")]
    [Fact(DisplayName = "Dado todos os Commands em Services.V1, então cada um tem Handler+Behavior+Result+Validation na mesma pasta")]
    public void TodoCommand_TemHandlerBehaviorResultValidation()
    {
        var faltando = new List<string>();
        foreach (var t in RequestTypes("Command"))
        {
            var folder = FolderForType(t);
            if (!Directory.Exists(folder)) { faltando.Add($"{t.FullName}: pasta {folder} não existe"); continue; }

            foreach (var sibling in new[] { "Handler", "Behavior", "Result", "Validation" })
            {
                var expected = Path.Combine(folder, $"{t.Name}{sibling}.cs");
                if (!File.Exists(expected))
                    faltando.Add($"{t.FullName}: faltando {t.Name}{sibling}.cs");
            }
        }
        faltando.Should().BeEmpty(string.Join("\n", faltando));
    }

    [Trait("Solucao", "Test")]
    [Trait("Acao", "Convencoes")]
    [Fact(DisplayName = "Dado todas as Queries em Services.V1, então cada uma tem Handler+Behavior+Result+Validation na mesma pasta")]
    public void TodaQuery_TemHandlerBehaviorResultValidation()
    {
        var faltando = new List<string>();
        foreach (var t in RequestTypes("Query"))
        {
            var folder = FolderForType(t);
            if (!Directory.Exists(folder)) { faltando.Add($"{t.FullName}: pasta {folder} não existe"); continue; }

            foreach (var sibling in new[] { "Handler", "Behavior", "Result", "Validation" })
            {
                var expected = Path.Combine(folder, $"{t.Name}{sibling}.cs");
                if (!File.Exists(expected))
                    faltando.Add($"{t.FullName}: faltando {t.Name}{sibling}.cs");
            }
        }
        faltando.Should().BeEmpty(string.Join("\n", faltando));
    }

    [Trait("Solucao", "Test")]
    [Trait("Acao", "Convencoes")]
    [Fact(DisplayName = "Dado todas as Notifications em Services.V1, então cada uma tem ao menos um Handler + Behavior + Validation na mesma pasta")]
    public void TodaNotification_TemHandlerBehaviorValidation()
    {
        var asm = typeof(Acme.Sistemas.Services.ServicesServiceCollection).Assembly;
        var notifications = asm.GetTypes()
            .Where(t => !t.IsAbstract
                && !t.IsInterface
                && typeof(INotification).IsAssignableFrom(t)
                && (t.Namespace?.StartsWith("Acme.Sistemas.Services.V1.") ?? false))
            .ToList();

        var faltando = new List<string>();
        foreach (var t in notifications)
        {
            var folder = FolderForType(t);
            if (!Directory.Exists(folder)) { faltando.Add($"{t.FullName}: pasta {folder} não existe"); continue; }

            // Handler em notificação: convenção pub/sub aceita 1+ handlers — basta haver pelo menos
            // um arquivo `*Handler.cs` na mesma pasta (ex.: AlertaEstoqueMinimoLogHandler.cs).
            var prefix = t.Name; // AlertaEstoqueMinimoNotification
            var prefixSemNotif = prefix.EndsWith("Notification", StringComparison.Ordinal)
                ? prefix[..^"Notification".Length]
                : prefix;
            var anyHandler = Directory.GetFiles(folder, $"{prefixSemNotif}*Handler.cs").Length > 0
                          || Directory.GetFiles(folder, $"{prefix}Handler.cs").Length > 0;
            if (!anyHandler)
                faltando.Add($"{t.FullName}: faltando handler (nenhum arquivo {prefixSemNotif}*Handler.cs em {folder})");

            foreach (var sibling in new[] { "Behavior", "Validation" })
            {
                var expected = Path.Combine(folder, $"{t.Name}{sibling}.cs");
                if (!File.Exists(expected))
                    faltando.Add($"{t.FullName}: faltando {t.Name}{sibling}.cs");
            }
        }
        faltando.Should().BeEmpty(string.Join("\n", faltando));
    }

    private static readonly HashSet<string> CamadasValidas = new(StringComparer.Ordinal)
    {
        "Api", "Services", "Core", "Domain", "Repository", "Infrastructure", "ExternalIntegration", "Test",
    };

    [Trait("Solucao", "Test")]
    [Trait("Acao", "Convencoes")]
    [Fact(
        Skip = "ativa após retrofit completo (Fase 4 do change padronizar-traits-displayname-tests)",
        DisplayName = "Dado um método [Fact]/[Theory] em UnitTest ou IntegrationTest, então tem DisplayName + Trait(Solucao) + Trait(Acao) válidos")]
    public void TodoTeste_TemDisplayNameESolucaoEAcao()
    {
        var assemblies = new[]
        {
            typeof(ConvencoesBlueprintTests).Assembly,
            typeof(Acme.Sistemas.IntegrationTest.Config.IntegrationTestBase).Assembly,
        };

        var faltando = new List<string>();
        foreach (var asm in assemblies)
        {
            foreach (var type in asm.GetTypes().Where(t => t.IsClass && t.IsPublic))
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    var fact = method.GetCustomAttribute<FactAttribute>(inherit: true);
                    if (fact is null) continue;

                    var loc = $"{type.FullName}.{method.Name}";

                    if (string.IsNullOrWhiteSpace(fact.DisplayName))
                        faltando.Add($"{loc}: faltando DisplayName em [Fact]/[Theory]");

                    var traits = ReadTraits(method);

                    if (!traits.TryGetValue("Solucao", out var solucao))
                        faltando.Add($"{loc}: faltando [Trait(\"Solucao\", <camada>)]");
                    else if (!CamadasValidas.Contains(solucao))
                        faltando.Add($"{loc}: Trait(\"Solucao\", \"{solucao}\") fora da allow-list ({string.Join(", ", CamadasValidas)})");

                    if (!traits.TryGetValue("Acao", out var acao) || string.IsNullOrWhiteSpace(acao))
                        faltando.Add($"{loc}: faltando [Trait(\"Acao\", <unidade>)]");
                }
            }
        }

        faltando.Should().BeEmpty(string.Join("\n", faltando));
    }

    private static Dictionary<string, string> ReadTraits(MethodInfo method)
    {
        // xUnit.TraitAttribute não expõe Name/Value como properties; lê via CustomAttributeData.
        var traits = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var data in method.GetCustomAttributesData())
        {
            if (data.AttributeType.Name != nameof(TraitAttribute)) continue;
            if (data.ConstructorArguments.Count < 2) continue;
            var key = data.ConstructorArguments[0].Value as string;
            var value = data.ConstructorArguments[1].Value as string;
            if (key is null) continue;
            traits[key] = value ?? string.Empty;
        }
        return traits;
    }
}
