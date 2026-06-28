using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Infrastructure.Databases.Migrations;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Valida o resultado das 14 migrations da Fase 1 do rh-fundacao (W1) — desde a criação
/// das 10 tabelas novas até o backfill dos funcionários legados em cargos/lotações/usuários
/// auto-criados. Os testes rodam contra a instância MySQL real do container Docker; o boot
/// do <see cref="IntegrationWebApplicationFactory"/> dispara o <c>MigrationRunner</c> que
/// aplica todas as migrations antes do primeiro assert.
/// </summary>
public class MigrationsRhFundacaoTests : IntegrationTestBase
{
    public MigrationsRhFundacaoTests(DockerEnvironment docker) : base(docker) { }

    private static readonly string[] TabelasRhNovas =
    {
        "jornadas", "cargos", "lotacoes", "departamentos",
        "historico_salarios", "beneficios_catalogo", "beneficios_funcionario",
        "dependentes", "escalas_funcionario", "cbos"
    };

    private static readonly string[] ColunasNovasFuncionario =
    {
        "cargo_id", "lotacao_id", "departamento_id", "tipo_contrato", "regime_remuneracao",
        "codigo_matricula", "pis", "ctps", "ctps_serie", "ctps_uf",
        "rg", "rg_orgao", "rg_uf", "estado_civil", "naturalidade", "nacionalidade",
        "endereco_json", "conta_bancaria_json"
    };

    private static readonly long[] MigrationsRhFundacaoVersoes =
    {
        20260628001, 20260628002, 20260628003, 20260628004, 20260628005,
        20260628006, 20260628007, 20260628008, 20260628009, 20260628010,
        20260628011, 20260628012, 20260628013, 20260628014
    };

    // ------------------------------------------------------------------- Schema

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "MigrationsRhFundacao")]
    [SkippableFact(DisplayName = "Dado boot da Factory, quando inspeciona INFORMATION_SCHEMA, então as 10 tabelas RH novas existem")]
    public async Task PosBoot_10TabelasRhNovas_Existem()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        foreach (var tabela in TabelasRhNovas)
        {
            var existe = await TabelaExiste(db, tabela);
            existe.Should().BeTrue($"a migration V2026062800x deve ter criado a tabela `{tabela}`");
        }
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "MigrationsRhFundacao")]
    [SkippableFact(DisplayName = "Dado boot da Factory, quando inspeciona funcionarios, então tem as 18 colunas RH novas + UK matrícula")]
    public async Task PosBoot_Funcionarios_TemColunasUkNovas()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        foreach (var coluna in ColunasNovasFuncionario)
        {
            var existe = await ColunaExiste(db, "funcionarios", coluna);
            existe.Should().BeTrue($"`funcionarios.{coluna}` deve ter sido adicionada pela V20260628011");
        }

        var ukExiste = await IndiceExiste(db, "funcionarios", "ux_funcionarios_tenant_matricula");
        ukExiste.Should().BeTrue("UNIQUE KEY (tenant_id, codigo_matricula) é exigência do eSocial");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "MigrationsRhFundacao")]
    [SkippableFact(DisplayName = "Dado boot da Factory, quando consulta __migrations, então as 14 versões 20260628xxx estão registradas")]
    public async Task PosBoot_14MigrationsRhFundacao_RegistradasNoTrace()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var registradas = await db.QueryAsync(
            "SELECT version FROM __migrations WHERE version BETWEEN 20260628001 AND 20260628999",
            r => r.GetInt64(0));

        registradas.Should().BeEquivalentTo(MigrationsRhFundacaoVersoes,
            "todas as 14 migrations da Fase 1 do rh-fundacao devem aparecer em __migrations");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "MigrationsRhFundacao")]
    [SkippableFact(DisplayName = "Dado migrations reaplicadas em sequência, quando observa estado, então são idempotentes e não duplicam dados")]
    public async Task Migrations_Reaplicadas_SaoIdempotentes()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db, "Tenant Idempotencia");

        // Reaplica a sequência de tabelas e backfill — todas guardadas por IF NOT EXISTS / WHERE NOT EXISTS.
        IMigration[] paraReaplicar =
        {
            new V20260628001_AddTabelaJornadas(),
            new V20260628011_AlterarFuncionariosAdicionarCamposRh(),
            new V20260628012_MigrarFuncionariosLegadosCargoDepto()
        };

        foreach (var m in paraReaplicar)
        {
            ExecutarUp(db, m);
            ExecutarUp(db, m); // chamada dupla para forçar caminho idempotente
        }

        var cargosNaoClass = await db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM cargos WHERE tenant_id = @t AND codigo = 'NAO-CLASS'",
            new Dictionary<string, object?> { ["@t"] = tenantId });

        cargosNaoClass.Should().Be(1, "INSERT...SELECT...WHERE NOT EXISTS evita duplicação do 'Não classificado'");

        var lotacoesSede = await db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM lotacoes WHERE tenant_id = @t AND nome = 'Sede'",
            new Dictionary<string, object?> { ["@t"] = tenantId });

        lotacoesSede.Should().Be(1, "lotação Sede default deve existir uma única vez por tenant");
    }

    // ----------------------------------------------------------------- Backfill funcional

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "MigrarFuncionariosLegadosCargoDepto")]
    [SkippableFact(DisplayName = "Dado funcionário legado com cargo+departamento texto e sem usuário, quando reaplica V20260628012+V20260628013, então cargo_id/lotacao_id/departamento_id/usuario_id ficam populados sem perda")]
    public async Task FuncionarioLegado_AposBackfill_TemFksPopuladasEUsuarioAutoCriado()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db, "Tenant Backfill");
        var funcionarioId = Guid.NewGuid();

        await db.ExecuteAsync(@"
            INSERT INTO funcionarios (id, tenant_id, nome_completo, cpf, cargo, departamento, status, created_at)
            VALUES (@id, @t, @nome, @cpf, 'Analista Pleno', 'TI', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = funcionarioId.ToString(),
                ["@t"] = tenantId,
                ["@nome"] = "Funcionario Legado",
                ["@cpf"] = "11122233344"
            });

        ExecutarUp(db, new V20260628012_MigrarFuncionariosLegadosCargoDepto());
        ExecutarUp(db, new V20260628013_CriarUsuariosDesativadosParaFuncionariosLegados());

        var ler = await db.QueryFirstOrDefaultAsync(@"
            SELECT cargo_id, departamento_id, lotacao_id, usuario_id, tipo_contrato, regime_remuneracao
            FROM funcionarios WHERE id = @id",
            r => new
            {
                CargoId = ReadId(r, 0),
                DepartamentoId = ReadId(r, 1),
                LotacaoId = ReadId(r, 2),
                UsuarioId = ReadId(r, 3),
                TipoContrato = r.IsDBNull(4) ? null : r.GetString(4),
                Regime = r.IsDBNull(5) ? null : r.GetString(5)
            },
            new Dictionary<string, object?> { ["@id"] = funcionarioId.ToString() });

        ler.Should().NotBeNull();
        ler!.CargoId.Should().NotBeNullOrEmpty("backfill deve criar/encontrar 'Analista Pleno' em cargos");
        ler.DepartamentoId.Should().NotBeNullOrEmpty("backfill deve criar/encontrar 'TI' em departamentos");
        ler.LotacaoId.Should().NotBeNullOrEmpty("default 'Sede' deve ser atribuído");
        ler.UsuarioId.Should().NotBeNullOrEmpty("usuário inativo deve ser auto-criado e vinculado");
        ler.TipoContrato.Should().Be("Clt");
        ler.Regime.Should().Be("Mensalista");

        // O cargo correspondente foi criado pela descrição
        var cargo = await db.QueryFirstOrDefaultAsync(
            "SELECT descricao FROM cargos WHERE id = @id",
            r => r.GetString(0),
            new Dictionary<string, object?> { ["@id"] = ler.CargoId! });
        cargo.Should().Be("Analista Pleno");

        // O usuário auto-criado segue o padrão de e-mail + Status=Inativo
        var usuario = await db.QueryFirstOrDefaultAsync(@"
            SELECT email, status FROM usuarios WHERE id = @id",
            r => new { Email = r.GetString(0), Status = r.GetByte(1) },
            new Dictionary<string, object?> { ["@id"] = ler.UsuarioId! });

        usuario.Should().NotBeNull();
        usuario!.Email.Should().StartWith("func-").And.EndWith("@auto.local");
        usuario.Status.Should().Be(0, "Status=Inativo (StatusAtivo.Inativo) protege contra login");

        // O campo texto legado continua preenchido — backfill é não-destrutivo
        var legado = await db.QueryFirstOrDefaultAsync(
            "SELECT cargo, departamento FROM funcionarios WHERE id = @id",
            r => new { Cargo = r.GetString(0), Departamento = r.GetString(1) },
            new Dictionary<string, object?> { ["@id"] = funcionarioId.ToString() });

        legado.Should().NotBeNull();
        legado!.Cargo.Should().Be("Analista Pleno", "campo texto antigo é preservado até remoção em W3");
        legado.Departamento.Should().Be("TI");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "MigrarFuncionariosLegadosCargoDepto")]
    [SkippableFact(DisplayName = "Dado funcionário legado sem cargo nem departamento, quando reaplica V20260628012, então recebe 'Não classificado' em ambos")]
    public async Task FuncionarioLegado_SemCargoDeptDefault_RecebeNaoClassificado()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db, "Tenant Sem Cargo");
        var funcionarioId = Guid.NewGuid();

        await db.ExecuteAsync(@"
            INSERT INTO funcionarios (id, tenant_id, nome_completo, cpf, status, created_at)
            VALUES (@id, @t, @nome, @cpf, 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = funcionarioId.ToString(),
                ["@t"] = tenantId,
                ["@nome"] = "Sem Cargo",
                ["@cpf"] = "55566677788"
            });

        ExecutarUp(db, new V20260628012_MigrarFuncionariosLegadosCargoDepto());

        var cargoCodigo = await db.QueryFirstOrDefaultAsync<string?>(@"
            SELECT c.codigo FROM funcionarios f JOIN cargos c ON c.id = f.cargo_id WHERE f.id = @id",
            r => r.IsDBNull(0) ? null : r.GetString(0),
            new Dictionary<string, object?> { ["@id"] = funcionarioId.ToString() });

        var deptoCodigo = await db.QueryFirstOrDefaultAsync<string?>(@"
            SELECT d.codigo FROM funcionarios f JOIN departamentos d ON d.id = f.departamento_id WHERE f.id = @id",
            r => r.IsDBNull(0) ? null : r.GetString(0),
            new Dictionary<string, object?> { ["@id"] = funcionarioId.ToString() });

        cargoCodigo.Should().Be("NAO-CLASS");
        deptoCodigo.Should().Be("NAO-CLASS");
    }

    // ---------------------------------------------------------------------- Helpers

    private static async Task<string> SeedTenantTeste(IDataConfiguration db, string razaoSocial)
    {
        var tenantId = Guid.NewGuid().ToString();
        // CNPJ aleatório por teste (apenas dígitos suficientes para o VARCHAR(18)) para
        // não colidir com `ux_tenants_cnpj` se vários testes rodarem na mesma DB.
        var cnpj = Guid.NewGuid().ToString("N")[..14];

        await db.ExecuteAsync(@"
            INSERT INTO tenants (id, razao_social, cnpj, plano, status, created_at)
            VALUES (@id, @razao, @cnpj, 'FREE', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = tenantId,
                ["@razao"] = razaoSocial,
                ["@cnpj"] = cnpj
            });

        return tenantId;
    }

    private static async Task<bool> TabelaExiste(IDataConfiguration db, string tabela)
    {
        var count = await db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM information_schema.tables
            WHERE table_schema = DATABASE() AND table_name = @t",
            new Dictionary<string, object?> { ["@t"] = tabela });
        return count > 0;
    }

    private static async Task<bool> ColunaExiste(IDataConfiguration db, string tabela, string coluna)
    {
        var count = await db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM information_schema.columns
            WHERE table_schema = DATABASE() AND table_name = @t AND column_name = @c",
            new Dictionary<string, object?> { ["@t"] = tabela, ["@c"] = coluna });
        return count > 0;
    }

    private static async Task<bool> IndiceExiste(IDataConfiguration db, string tabela, string indice)
    {
        var count = await db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM information_schema.statistics
            WHERE table_schema = DATABASE() AND table_name = @t AND index_name = @i",
            new Dictionary<string, object?> { ["@t"] = tabela, ["@i"] = indice });
        return count > 0;
    }

    // CHAR(36) é devolvido pelo MySqlConnector como Guid (não como string), então qualquer
    // leitura genérica de coluna-ID precisa tolerar ambos os tipos.
    private static string? ReadId(IDataRecord r, int ordinal)
    {
        if (r.IsDBNull(ordinal)) return null;
        var raw = r.GetValue(ordinal);
        return raw switch
        {
            Guid g => g.ToString(),
            string s => s,
            _ => raw.ToString()
        };
    }

    private static void ExecutarUp(IDataConfiguration db, IMigration migration)
    {
        using var conn = db.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            migration.Up(conn, tx);
            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}
