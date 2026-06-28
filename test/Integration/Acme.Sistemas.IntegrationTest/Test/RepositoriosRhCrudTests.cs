using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Round-trip dos 4 repositórios introduzidos pela Fase 2 (Cargo, Departamento, Lotacao,
/// Jornada): Add → Get/List → Update → Delete (soft) e isolamento multi-tenant herdado
/// do <see cref="BaseRepository{T}"/>. Cada teste reusa <see cref="IMutableTenantContext"/>
/// para isolar o ambiente em um tenant transitório, evitando colisão com seeds globais.
/// </summary>
public class RepositoriosRhCrudTests : IntegrationTestBase
{
    public RepositoriosRhCrudTests(DockerEnvironment docker) : base(docker) { }

    // ---------------------------------------------------------- Cargo

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "CargoRepository")]
    [SkippableFact(DisplayName = "Dado cargo inserido, quando GetByCodigo, então retorna o cargo com os mesmos campos")]
    public async Task Cargo_AddEntaoGetByCodigo_RetornaEntidadeCompleta()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;

        var tenantId = await PrepararTenantTeste(sp);
        var repo = sp.GetRequiredService<ICargoRepository>();

        var cargo = new Cargo
        {
            Codigo = "DEV-SR",
            Descricao = "Desenvolvedor Sênior",
            CodigoCbo = "212405",
            SalarioBaseSugerido = 12_500m,
            Ativo = true,
        };
        await repo.AddAsync(cargo);

        var lido = await repo.GetByCodigoAsync("DEV-SR");

        lido.Should().NotBeNull();
        lido!.Id.Should().Be(cargo.Id);
        lido.TenantId.Should().Be(tenantId);
        lido.Descricao.Should().Be("Desenvolvedor Sênior");
        lido.CodigoCbo.Should().Be("212405");
        lido.SalarioBaseSugerido.Should().Be(12_500m);
        lido.Ativo.Should().BeTrue();
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "CargoRepository")]
    [SkippableFact(DisplayName = "Dado cargo inexistente, quando GetByCodigo, então retorna null sem lançar")]
    public async Task Cargo_GetByCodigoInexistente_RetornaNull()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var repo = sp.GetRequiredService<ICargoRepository>();
        var lido = await repo.GetByCodigoAsync("CODIGO-QUE-NAO-EXISTE");

        lido.Should().BeNull();
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "CargoRepository")]
    [SkippableFact(DisplayName = "Dado cargo deletado (soft), quando GetById, então retorna null e CountAsync ignora")]
    public async Task Cargo_AposSoftDelete_NaoApareceEmGetByIdNemCount()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var repo = sp.GetRequiredService<ICargoRepository>();
        var cargo = new Cargo { Codigo = "TMP-DEL", Descricao = "Temporário a deletar", Ativo = true };
        await repo.AddAsync(cargo);

        var antes = await repo.CountAsync();
        await repo.DeleteAsync(cargo.Id);
        var depois = await repo.CountAsync();

        depois.Should().Be(antes - 1);
        (await repo.GetByIdAsync(cargo.Id)).Should().BeNull();
        (await repo.GetByCodigoAsync("TMP-DEL")).Should().BeNull("soft delete deve respeitar deleted_at IS NULL");
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "CargoRepository")]
    [SkippableFact(DisplayName = "Dado cargo inserido no Tenant A, quando lista pelo Tenant B, então não aparece (isolamento BaseRepository)")]
    public async Task Cargo_IsolamentoTenantAVisivelEmBContextoErrado_NaoVaza()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();

        // Tenant A insere cargo em escopo isolado.
        Guid tenantA;
        Guid cargoIdA;
        using (var scopeA = Factory.Services.CreateScope())
        {
            var spA = scopeA.ServiceProvider;
            tenantA = await PrepararTenantTeste(spA);
            var repoA = spA.GetRequiredService<ICargoRepository>();
            var cargo = new Cargo { Codigo = "ISO-A", Descricao = "Cargo só do Tenant A", Ativo = true };
            await repoA.AddAsync(cargo);
            cargoIdA = cargo.Id;
        }

        // Tenant B em outro escopo: NUNCA deve ver o cargo de A.
        using (var scopeB = Factory.Services.CreateScope())
        {
            var spB = scopeB.ServiceProvider;
            var tenantB = await PrepararTenantTeste(spB);
            tenantB.Should().NotBe(tenantA);

            var repoB = spB.GetRequiredService<ICargoRepository>();
            (await repoB.GetByCodigoAsync("ISO-A")).Should().BeNull(
                "BaseRepository.GetByCodigo filtra por tenant_id = TenantContext.TenantId");
            (await repoB.GetByIdAsync(cargoIdA)).Should().BeNull(
                "BaseRepository.GetByIdAsync também filtra por tenant_id");
        }
    }

    // ---------------------------------------------------------- Departamento

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "DepartamentoRepository")]
    [SkippableFact(DisplayName = "Dado departamento com CentroDeCustoId, quando Add+GetByCodigo, então preserva o vínculo")]
    public async Task Departamento_AddComCentroDeCusto_PreservaVinculo()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var repo = sp.GetRequiredService<IDepartamentoRepository>();
        var ccId = Guid.NewGuid();
        var depto = new Departamento { Codigo = "TI", Nome = "Tecnologia", CentroDeCustoId = ccId, Ativo = true };
        await repo.AddAsync(depto);

        var lido = await repo.GetByCodigoAsync("TI");
        lido.Should().NotBeNull();
        lido!.CentroDeCustoId.Should().Be(ccId);
        lido.Nome.Should().Be("Tecnologia");
    }

    // ---------------------------------------------------------- Lotação

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "LotacaoRepository")]
    [SkippableFact(DisplayName = "Dado lotação com endereco_json e CNPJ próprio, quando Add+GetByNome, então preserva ambos")]
    public async Task Lotacao_AddComEnderecoJsonECnpj_PreservaTodosCampos()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var repo = sp.GetRequiredService<ILotacaoRepository>();
        var lot = new Lotacao
        {
            Nome = "Filial SP",
            Cnpj = "11222333000181",
            EnderecoJson = "{\"cidade\":\"São Paulo\",\"uf\":\"SP\"}",
            Ativo = true,
        };
        await repo.AddAsync(lot);

        var lido = await repo.GetByNomeAsync("Filial SP");
        lido.Should().NotBeNull();
        lido!.Cnpj.Should().Be("11222333000181");
        lido.EnderecoJson.Should().Contain("São Paulo").And.Contain("SP");
    }

    // ---------------------------------------------------------- Jornada

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "JornadaRepository")]
    [SkippableFact(DisplayName = "Dado jornada Escala12x36 com tolerância 15, quando Add+GetByNome, então TipoJornada enum desserializa corretamente do VARCHAR")]
    public async Task Jornada_AddEscala12x36_TipoEnumIdaEVoltaCorretamente()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var repo = sp.GetRequiredService<IJornadaRepository>();
        var j = new Jornada
        {
            Nome = "12x36 Enfermagem",
            Tipo = TipoJornada.Escala12x36,
            CargaSemanalHoras = 42m,
            CargaDiariaHoras = 12m,
            JanelasJson = "[{\"dia\":\"seg\",\"entrada\":\"07:00\",\"saida\":\"19:00\"}]",
            ToleranciaMinutos = 15,
            Ativo = true,
        };
        await repo.AddAsync(j);

        var lido = await repo.GetByNomeAsync("12x36 Enfermagem");
        lido.Should().NotBeNull();
        lido!.Tipo.Should().Be(TipoJornada.Escala12x36, "enum deve ser persistido como string e re-desserializado");
        lido.CargaSemanalHoras.Should().Be(42m);
        lido.CargaDiariaHoras.Should().Be(12m);
        lido.ToleranciaMinutos.Should().Be(15);
        lido.JanelasJson.Should().Contain("07:00");
    }

    [Trait("Solucao", "Repository")]
    [Trait("Acao", "JornadaRepository")]
    [SkippableFact(DisplayName = "Dado jornada existente, quando Update altera campos, então leitura subsequente reflete os novos valores")]
    public async Task Jornada_Update_PersisteAlteracoes()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var sp = scope.ServiceProvider;
        await PrepararTenantTeste(sp);

        var repo = sp.GetRequiredService<IJornadaRepository>();
        var j = new Jornada
        {
            Nome = "44h Original",
            Tipo = TipoJornada.Fixa,
            CargaSemanalHoras = 44m,
            JanelasJson = "[]",
            ToleranciaMinutos = 5,
            Ativo = true,
        };
        await repo.AddAsync(j);

        var carregada = await repo.GetByNomeAsync("44h Original");
        carregada!.ToleranciaMinutos = 20;
        carregada.Ativo = false;
        await repo.UpdateAsync(carregada);

        var releitura = await repo.GetByIdAsync(j.Id);
        releitura.Should().NotBeNull();
        releitura!.ToleranciaMinutos.Should().Be(20);
        releitura.Ativo.Should().BeFalse();
    }

    // ---------------------------------------------------------------------- Helpers

    /// <summary>
    /// Cria um tenant transitório direto na tabela <c>tenants</c> e configura
    /// <see cref="IMutableTenantContext"/> para apontar para ele. Os repositórios
    /// herdados de <c>BaseRepository</c> passam a operar nesse tenant.
    /// </summary>
    private static async Task<Guid> PrepararTenantTeste(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<Acme.Sistemas.Infrastructure.Databases.Configuration.IDataConfiguration>();
        var tenantId = Guid.NewGuid();
        var cnpj = Guid.NewGuid().ToString("N")[..14];

        await db.ExecuteAsync(@"
            INSERT INTO tenants (id, razao_social, cnpj, plano, status, created_at)
            VALUES (@id, @razao, @cnpj, 'FREE', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = tenantId.ToString(),
                ["@razao"] = "Tenant Repo Test " + cnpj[..6],
                ["@cnpj"] = cnpj
            });

        var ctx = sp.GetRequiredService<IMutableTenantContext>();
        ctx.Override(tenantId);

        return tenantId;
    }
}
