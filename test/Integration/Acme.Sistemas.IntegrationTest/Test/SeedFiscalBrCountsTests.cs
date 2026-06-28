using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Valida que as migrations de catálogos estáticos brasileiros (UFs, CFOPs, CSTs, LC116)
/// populam as tabelas com as contagens esperadas após o boot. Catálogos são de referência
/// nacional (não tenant-scoped), então os repositórios são resolvidos diretamente.
/// </summary>
public class SeedFiscalBrCountsTests : IntegrationTestBase
{
    public SeedFiscalBrCountsTests(DockerEnvironment docker) : base(docker) { }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "SeedFiscalBr")]
    [SkippableFact(DisplayName = "Dado migrations aplicadas, quando consulta catálogos BR, então UFs=27 e demais catálogos populados")]
    public async Task Catalogos_PosMigration_TemContagensEsperadas()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;

        var ufs = await sp.GetRequiredService<IUfRepository>().CountAsync();
        ufs.Should().Be(27, "as 27 UFs são semeadas inline na migration");

        var cfops = await sp.GetRequiredService<ICfopRepository>().CountAsync();
        cfops.Should().BeGreaterThan(20, "subset curado de CFOPs é semeado inline");

        var lc116 = await sp.GetRequiredService<ICodigoServicoLc116Repository>().CountAsync();
        lc116.Should().BeGreaterThan(40, "subset curado de códigos LC 116 cobre os 40 grupos");

        var cstRepo = sp.GetRequiredService<ICstRepository>();
        (await cstRepo.CountAsync("icms")).Should().BeGreaterThan(0);
        (await cstRepo.CountAsync("pis")).Should().BeGreaterThan(0);
        (await cstRepo.CountAsync("cofins")).Should().BeGreaterThan(0);
        (await cstRepo.CountAsync("ipi")).Should().BeGreaterThan(0);
    }
}
