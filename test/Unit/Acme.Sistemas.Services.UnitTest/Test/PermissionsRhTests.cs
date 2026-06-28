using Acme.Sistemas.Core.Const;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

/// <summary>
/// Garante que as constantes de permissão adicionadas pela Fase 2 do rh-fundacao
/// (8 Recursos.Rh* + Acoes.GerirEquipe) aparecem corretamente em
/// <see cref="Permissions.All()"/> — o <see cref="Acme.Sistemas.Atena.Api.Hosted.PermissionsSeedHostedService"/>
/// usa esse cross-product para popular a tabela <c>permissions</c> no boot, então qualquer
/// constante esquecida significa permissão ausente em runtime.
/// </summary>
public class PermissionsRhTests
{
    private static readonly string[] RecursosRhEsperados =
    {
        Permissions.Recursos.Rh,
        Permissions.Recursos.RhFuncionario,
        Permissions.Recursos.RhJornada,
        Permissions.Recursos.RhCargo,
        Permissions.Recursos.RhLotacao,
        Permissions.Recursos.RhBeneficio,
        Permissions.Recursos.RhDependente,
        Permissions.Recursos.RhDepartamento,
    };

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PermissionsRh")]
    [Fact(DisplayName = "Dado as 8 constantes Recursos.Rh*, quando consulta, então têm o prefixo 'rh' e código kebab-case")]
    public void RecursosRh_TodasConstantes_PrefixoCorreto()
    {
        Permissions.Recursos.Rh.Should().Be("rh");
        Permissions.Recursos.RhFuncionario.Should().Be("rh-funcionario");
        Permissions.Recursos.RhJornada.Should().Be("rh-jornada");
        Permissions.Recursos.RhCargo.Should().Be("rh-cargo");
        Permissions.Recursos.RhLotacao.Should().Be("rh-lotacao");
        Permissions.Recursos.RhBeneficio.Should().Be("rh-beneficio");
        Permissions.Recursos.RhDependente.Should().Be("rh-dependente");
        Permissions.Recursos.RhDepartamento.Should().Be("rh-departamento");
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PermissionsRh")]
    [Fact(DisplayName = "Dado Acoes.GerirEquipe, quando consulta, então é 'gerir-equipe' (kebab-case)")]
    public void GerirEquipe_Constante_KebabCase()
    {
        Permissions.Acoes.GerirEquipe.Should().Be("gerir-equipe");
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PermissionsRh")]
    [Fact(DisplayName = "Dado Permissions.All(), quando filtra por recursos RH, então contém todos os 8 recursos × todas as ações (>= 72 códigos)")]
    public void PermissionsAll_RecursosRh_PresentesNoCrossProduct()
    {
        var todos = Permissions.All();

        foreach (var recurso in RecursosRhEsperados)
        {
            var codigos = todos.Where(c => c.StartsWith(recurso + ":", StringComparison.Ordinal)).ToList();
            codigos.Should().NotBeEmpty($"recurso `{recurso}` deve aparecer no produto cartesiano de Permissions.All()");
            codigos.Should().Contain(recurso + ":ler");
            codigos.Should().Contain(recurso + ":criar");
            codigos.Should().Contain(recurso + ":editar");
            codigos.Should().Contain(recurso + ":excluir");
            codigos.Should().Contain(recurso + ":gerir-equipe",
                "GerirEquipe foi adicionada em Acoes e deve aparecer em todas as combinações");
        }

        var totalRh = todos.Count(c => RecursosRhEsperados.Any(r => c.StartsWith(r + ":", StringComparison.Ordinal)));
        totalRh.Should().BeGreaterThanOrEqualTo(8 * 9,
            "8 recursos RH × 9 ações cadastradas em Acoes (Ler/Criar/Editar/Excluir/Aprovar/Faturar/Cancelar/Exportar/SeedTenant/GerirEquipe — mas pelo menos 9 efetivas)");
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PermissionsRh")]
    [Theory(DisplayName = "Dado Permissions.Of(recurso, acao) para combinações RH novas, quando concatena, então segue formato 'recurso:acao'")]
    [InlineData("rh-funcionario", "gerir-equipe", "rh-funcionario:gerir-equipe")]
    [InlineData("rh-jornada", "criar", "rh-jornada:criar")]
    [InlineData("rh-beneficio", "excluir", "rh-beneficio:excluir")]
    [InlineData("rh-dependente", "ler", "rh-dependente:ler")]
    [InlineData("rh", "ler", "rh:ler")]
    public void Of_RhRecursoEAcao_ConcatenaComDoisPontos(string recurso, string acao, string esperado)
    {
        Permissions.Of(recurso, acao).Should().Be(esperado);
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PermissionsRh")]
    [Fact(DisplayName = "Dado todas as constantes Recursos, quando enumera via reflexão, então RH aparecem todos os 8 (regressão contra typo/remoção acidental)")]
    public void Recursos_Enumeracao_Contem8RecursosRh()
    {
        var fields = typeof(Permissions.Recursos).GetFields()
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => (string)f.GetRawConstantValue()!)
            .ToList();

        foreach (var recursoRh in RecursosRhEsperados)
        {
            fields.Should().Contain(recursoRh, $"`{recursoRh}` deve permanecer declarado em Permissions.Recursos");
        }
    }

    [Trait("Solucao", "Core")]
    [Trait("Acao", "PermissionsRh")]
    [Fact(DisplayName = "Dado Permissions.All(), quando procura códigos duplicados, então não há duplicatas")]
    public void PermissionsAll_NaoTemDuplicatas()
    {
        var todos = Permissions.All();
        var unicos = todos.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        unicos.Should().HaveCount(todos.Count,
            "Permissions.All() retorna o produto cartesiano de Recursos × Acoes — duplicatas indicariam constante repetida em Permissions.cs");
    }
}
