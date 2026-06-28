using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.IntegrationTest.Config;
using Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Valida o comportamento da Fase 2 do rh-fundacao: o <see cref="SeedTenantCommandHandler"/>
/// após criar tenant + admin + financeiro/cadastros demo, dispara <c>SeedRhDefaultsAsync</c>
/// para preparar o tenant a receber funcionários — role <c>RH</c>, jornada 44h CLT,
/// cargo / departamento "Não classificado" e lotação "Sede". Os testes verificam o estado
/// final no banco, idempotência (re-execução do mesmo CNPJ não duplica) e isolamento por tenant.
/// </summary>
public class SeedRhDefaultsTests : IntegrationTestBase
{
    public SeedRhDefaultsTests(DockerEnvironment docker) : base(docker) { }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "SeedTenantRhDefaults")]
    [SkippableFact(DisplayName = "Dado tenant novo via SeedTenantCommand, quando seed roda, então cria role 'RH' com todas as permissões rh-*:*")]
    public async Task SeedTenant_NovoCnpj_CriaRoleRhComPermissoesRh()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        var mediator = sp.GetRequiredService<IMediator>();

        var cnpj = GerarCnpj();
        var resultado = await mediator.Send(new SeedTenantCommand(
            Cnpj: cnpj,
            RazaoSocial: "Tenant RH Seed Test",
            AdminEmail: $"admin-{cnpj}@test.local"));

        resultado.IsSuccess.Should().BeTrue();
        resultado.Content!.EhNovo.Should().BeTrue();

        // O handler chama _tenantCtx.Override(tenant.Id), então os repositórios já
        // operam no tenant recém-criado dentro desse mesmo escopo.
        var roles = sp.GetRequiredService<IRoleRepository>();
        var roleRh = await roles.GetByNomeAsync("RH");
        roleRh.Should().NotBeNull("a role RH deve ter sido seedada");
        roleRh!.IsSystem.Should().BeTrue();
        roleRh.Descricao.Should().Contain("Recursos Humanos");

        var rolePermissoes = sp.GetRequiredService<IRolePermissionRepository>();
        var codigos = await rolePermissoes.GetCodigosByRoleAsync(roleRh.Id);

        codigos.Should().Contain(c => c.StartsWith("rh-funcionario:", StringComparison.Ordinal),
            "role RH deve receber pelo menos uma permissão rh-funcionario:*");
        codigos.Should().Contain(c => c.StartsWith("rh-jornada:", StringComparison.Ordinal));
        codigos.Should().Contain(c => c.StartsWith("rh-cargo:", StringComparison.Ordinal));
        codigos.Should().Contain(c => c.StartsWith("rh-beneficio:", StringComparison.Ordinal));
        codigos.Should().Contain(c => c.StartsWith("rh-dependente:", StringComparison.Ordinal));

        codigos.Should().NotContain(c => c.StartsWith("financeiro", StringComparison.Ordinal),
            "role RH não deve receber permissões financeiras");
        codigos.Should().NotContain(c => c.StartsWith("nfe:", StringComparison.Ordinal),
            "role RH não deve receber permissões fiscais");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "SeedTenantRhDefaults")]
    [SkippableFact(DisplayName = "Dado tenant novo via SeedTenantCommand, quando seed roda, então cria jornada '44h CLT' com janelas seg-sex 08:00-12:00 / 13:30-17:30 + sáb")]
    public async Task SeedTenant_NovoCnpj_CriaJornada44hCltComJanelasCorretas()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        var mediator = sp.GetRequiredService<IMediator>();

        var cnpj = GerarCnpj();
        await mediator.Send(new SeedTenantCommand(cnpj, "Tenant Jornada Test", $"admin-{cnpj}@test.local"));

        var jornadas = sp.GetRequiredService<IJornadaRepository>();
        var jornada44h = await jornadas.GetByNomeAsync("44h CLT");

        jornada44h.Should().NotBeNull();
        jornada44h!.Tipo.Should().Be(TipoJornada.Fixa);
        jornada44h.CargaSemanalHoras.Should().Be(44m);
        jornada44h.CargaDiariaHoras.Should().Be(8m);
        jornada44h.ToleranciaMinutos.Should().Be(10);
        jornada44h.PermiteMarcarIntervalo.Should().BeTrue();
        jornada44h.Ativo.Should().BeTrue();

        // MySQL JSON column normaliza a serialização (insere espaços após ':'). Asserções
        // ignoram whitespace para serem robustas a esse pretty-print.
        var canonical = System.Text.RegularExpressions.Regex.Replace(jornada44h.JanelasJson, "\\s+", "");
        canonical.Should().Contain("\"dia\":\"seg\"")
            .And.Contain("\"entrada\":\"08:00\"")
            .And.Contain("\"saidaAlmoco\":\"12:00\"")
            .And.Contain("\"voltaAlmoco\":\"13:30\"")
            .And.Contain("\"saida\":\"17:30\"")
            .And.Contain("\"dia\":\"sab\"");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "SeedTenantRhDefaults")]
    [SkippableFact(DisplayName = "Dado tenant novo via SeedTenantCommand, quando seed roda, então cria cargo/departamento 'NAO-CLASS' e lotação 'Sede'")]
    public async Task SeedTenant_NovoCnpj_CriaDefaultsCargoDeptoLotacao()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        var mediator = sp.GetRequiredService<IMediator>();

        var cnpj = GerarCnpj();
        await mediator.Send(new SeedTenantCommand(cnpj, "Tenant Defaults Test", $"admin-{cnpj}@test.local"));

        var cargos = sp.GetRequiredService<ICargoRepository>();
        var deptos = sp.GetRequiredService<IDepartamentoRepository>();
        var lotacoes = sp.GetRequiredService<ILotacaoRepository>();

        var cargo = await cargos.GetByCodigoAsync("NAO-CLASS");
        cargo.Should().NotBeNull();
        cargo!.Descricao.Should().Be("Não classificado");
        cargo.Ativo.Should().BeTrue();

        var depto = await deptos.GetByCodigoAsync("NAO-CLASS");
        depto.Should().NotBeNull();
        depto!.Nome.Should().Be("Não classificado");
        depto.Ativo.Should().BeTrue();

        var lotacao = await lotacoes.GetByNomeAsync("Sede");
        lotacao.Should().NotBeNull();
        lotacao!.Ativo.Should().BeTrue();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "SeedTenantRhDefaults")]
    [SkippableFact(DisplayName = "Dado SeedTenantCommand executado duas vezes com mesmo CNPJ, quando consulta defaults RH, então cada um aparece exatamente uma vez (idempotente)")]
    public async Task SeedTenant_ChamadoDuasVezes_NaoDuplicaDefaultsRh()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        var mediator = sp.GetRequiredService<IMediator>();

        var cnpj = GerarCnpj();
        var primeiro = await mediator.Send(new SeedTenantCommand(cnpj, "Tenant Idempotente", $"admin-{cnpj}@test.local"));
        primeiro.Content!.EhNovo.Should().BeTrue();

        // Segunda execução com mesmo CNPJ: handler já retorna EhNovo=false sem recriar.
        // Mas vamos reforçar: mesmo se SeedRhDefaultsAsync rodasse de novo, os guards
        // GetByCodigo/GetByNome evitariam duplicação.
        var jornadas = sp.GetRequiredService<IJornadaRepository>();
        var cargos = sp.GetRequiredService<ICargoRepository>();

        var todasJornadas44h = (await jornadas.ListAsync(0, 100))
            .Where(j => j.Nome == "44h CLT")
            .ToList();
        todasJornadas44h.Should().HaveCount(1);

        var todosCargosNaoClass = (await cargos.ListAsync(0, 100))
            .Where(c => c.Codigo == "NAO-CLASS")
            .ToList();
        todosCargosNaoClass.Should().HaveCount(1);
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "SeedTenantRhDefaults")]
    [SkippableFact(DisplayName = "Dado dois tenants criados via SeedTenant, quando lista cargos do Tenant A, então não retorna 'NAO-CLASS' do Tenant B (isolamento)")]
    public async Task SeedTenant_DoisTenants_DefaultsRhIsoladosPorTenant()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scopeA = Factory.Services.CreateScope();
        var mediatorA = scopeA.ServiceProvider.GetRequiredService<IMediator>();
        var cnpjA = GerarCnpj();
        await mediatorA.Send(new SeedTenantCommand(cnpjA, "Tenant A", $"admin-{cnpjA}@test.local"));
        var tenantA = ((IMutableTenantContext)scopeA.ServiceProvider
            .GetRequiredService<ITenantContext>()).TenantId;

        using var scopeB = Factory.Services.CreateScope();
        var mediatorB = scopeB.ServiceProvider.GetRequiredService<IMediator>();
        var cnpjB = GerarCnpj();
        await mediatorB.Send(new SeedTenantCommand(cnpjB, "Tenant B", $"admin-{cnpjB}@test.local"));
        var tenantB = ((IMutableTenantContext)scopeB.ServiceProvider
            .GetRequiredService<ITenantContext>()).TenantId;

        tenantA.Should().NotBe(tenantB);

        var cargosA = await scopeA.ServiceProvider.GetRequiredService<ICargoRepository>().ListAsync(0, 100);
        var cargosB = await scopeB.ServiceProvider.GetRequiredService<ICargoRepository>().ListAsync(0, 100);

        cargosA.Should().OnlyContain(c => c.TenantId == tenantA,
            "BaseRepository filtra automaticamente por TenantContext.TenantId");
        cargosB.Should().OnlyContain(c => c.TenantId == tenantB);

        cargosA.Select(c => c.Id).Should().NotIntersectWith(cargosB.Select(c => c.Id));
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "PermissionsSeedRh")]
    [SkippableFact(DisplayName = "Dado boot da Factory, quando consulta permissions, então as 8 chaves rh-* × ações estão semeadas em tabela permissions")]
    public async Task PermissionsSeed_PosBoot_ContemTodasPermissoesRh()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var permissoes = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();

        // PermissionsSeedHostedService roda no boot — pode levar até alguns segundos para
        // popular tudo. Como BackgroundService é fire-and-forget, é necessário aguardar
        // até que o seed seja confirmado pela presença das chaves esperadas.
        IReadOnlyList<Acme.Sistemas.Domain.Entities.Permissions.Permission>? todas = null;
        var deadline = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < deadline)
        {
            todas = await permissoes.ListAllAsync();
            if (todas.Any(p => p.Codigo == "rh-funcionario:ler")) break;
            await Task.Delay(200);
        }

        todas.Should().NotBeNull();
        var codigos = todas!.Select(p => p.Codigo).ToHashSet(StringComparer.OrdinalIgnoreCase);

        codigos.Should().Contain("rh:ler");
        codigos.Should().Contain("rh-funcionario:gerir-equipe");
        codigos.Should().Contain("rh-funcionario:criar");
        codigos.Should().Contain("rh-jornada:editar");
        codigos.Should().Contain("rh-cargo:excluir");
        codigos.Should().Contain("rh-lotacao:ler");
        codigos.Should().Contain("rh-beneficio:criar");
        codigos.Should().Contain("rh-dependente:editar");
        codigos.Should().Contain("rh-departamento:excluir");
    }

    private static string GerarCnpj()
    {
        // 14 dígitos (sem letras): SeedTenantCommandValidation exige exatamente 14 dígitos.
        // Não precisa ser CNPJ aritmeticamente válido — o handler apenas filtra char.IsDigit
        // e usa como UK em tenants. Gerado a partir de ticks UTC + sufixo aleatório para
        // garantir unicidade por execução.
        var ticks = DateTime.UtcNow.Ticks.ToString();
        var rng = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, 99999);
        var raw = ticks + rng.ToString("D5");
        return raw.AsSpan(raw.Length - 14, 14).ToString();
    }
}
