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

    [Fact]
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

    [Fact]
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

    [Fact]
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

}
