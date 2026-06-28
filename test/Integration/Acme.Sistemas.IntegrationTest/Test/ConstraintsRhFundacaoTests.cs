using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.IntegrationTest.Config;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Xunit;

namespace Acme.Sistemas.IntegrationTest.Test;

/// <summary>
/// Valida as mensagens de erro retornadas pelo MySQL quando contratos do schema da
/// Fase 1 do rh-fundacao são violados: unique keys compostas, foreign keys, NOT NULL
/// e validação de JSON nativo. Cada teste assert no <c>MySqlException.Number</c>
/// canônico do MySQL e em fragmentos estáveis da mensagem (nome do índice ou da FK),
/// para que reescrita das migrations preserve nomes de constraints.
/// </summary>
public class ConstraintsRhFundacaoTests : IntegrationTestBase
{
    public ConstraintsRhFundacaoTests(DockerEnvironment docker) : base(docker) { }

    // Códigos do MySQL (estáveis entre 5.7 e 8.x) que usamos em assertions.
    private const int ER_DUP_ENTRY = 1062;            // viola UNIQUE
    private const int ER_NO_REFERENCED_ROW_2 = 1452;  // viola FOREIGN KEY no INSERT/UPDATE
    private const int ER_BAD_NULL_ERROR = 1048;       // viola NOT NULL
    private const int ER_INVALID_JSON_TEXT = 3140;    // INSERT em coluna JSON com payload inválido

    // ----------------------------------------------------- UNIQUE KEY violations

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado funcionário com matrícula X, quando insere outro funcionário no mesmo tenant com matrícula X, então MySqlException(1062) cita ux_funcionarios_tenant_matricula")]
    public async Task Funcionario_MatriculaDuplicadaNoMesmoTenant_RejeitadaComIndiceCitado()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        await InserirFuncionarioComMatricula(db, tenantId, "MAT-001", cpf: "11111111111");

        var act = () => InserirFuncionarioComMatricula(db, tenantId, "MAT-001", cpf: "22222222222");

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_DUP_ENTRY);
        ex.Message.Should().Contain("ux_funcionarios_tenant_matricula",
            "preservar o nome do índice na mensagem é importante para diagnóstico operacional");
        ex.Message.Should().Contain("MAT-001");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado funcionário com matrícula X no Tenant A, quando insere matrícula X no Tenant B, então sucesso (UK é composta por tenant)")]
    public async Task Funcionario_MatriculaIgualEmTenantsDiferentes_AceitaPorqueUkEhComposta()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantA = await SeedTenantTeste(db);
        var tenantB = await SeedTenantTeste(db);
        await InserirFuncionarioComMatricula(db, tenantA, "MAT-XYZ", cpf: "33333333333");

        var act = () => InserirFuncionarioComMatricula(db, tenantB, "MAT-XYZ", cpf: "33333333333");

        await act.Should().NotThrowAsync("UK é (tenant_id, codigo_matricula), não global");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado cargo com código DEV no tenant, quando insere outro cargo com código DEV no mesmo tenant, então MySqlException(1062) cita ux_cargos_tenant_codigo")]
    public async Task Cargo_CodigoDuplicadoNoMesmoTenant_RejeitadaComIndiceCitado()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        await InserirCargo(db, tenantId, "DEV", "Desenvolvedor");

        var act = () => InserirCargo(db, tenantId, "DEV", "Outro Dev");

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_DUP_ENTRY);
        ex.Message.Should().Contain("ux_cargos_tenant_codigo");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado departamento com código TI no tenant, quando insere outro com código TI, então MySqlException(1062) cita ux_departamentos_tenant_codigo")]
    public async Task Departamento_CodigoDuplicadoNoMesmoTenant_RejeitadaComIndiceCitado()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        await db.ExecuteAsync(@"
            INSERT INTO departamentos (id, tenant_id, codigo, nome, ativo, created_at)
            VALUES (UUID(), @t, 'TI', 'Tecnologia', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?> { ["@t"] = tenantId });

        var act = () => db.ExecuteAsync(@"
            INSERT INTO departamentos (id, tenant_id, codigo, nome, ativo, created_at)
            VALUES (UUID(), @t, 'TI', 'Repetido', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?> { ["@t"] = tenantId });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_DUP_ENTRY);
        ex.Message.Should().Contain("ux_departamentos_tenant_codigo");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado benefício de catálogo com código VT, quando insere outro com VT no mesmo tenant, então MySqlException(1062) cita ux_beneficios_catalogo_tenant_codigo")]
    public async Task BeneficioCatalogo_CodigoDuplicadoNoMesmoTenant_RejeitadaComIndiceCitado()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        await InserirBeneficioCatalogo(db, tenantId, "VT", "Vale Transporte");

        var act = () => InserirBeneficioCatalogo(db, tenantId, "VT", "Vale Trans 2");

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_DUP_ENTRY);
        ex.Message.Should().Contain("ux_beneficios_catalogo_tenant_codigo");
    }

    // -------------------------------------------------- FOREIGN KEY violations

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado beneficio_funcionario apontando para catalogo_id inexistente, quando insere, então MySqlException(1452) cita fk_beneficios_func_cat")]
    public async Task BeneficioFuncionario_CatalogoIdInexistente_RejeitadaComFkCitada()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        var funcionarioId = await InserirFuncionarioComMatricula(db, tenantId, "BF-001", cpf: "44444444444");

        var act = () => db.ExecuteAsync(@"
            INSERT INTO beneficios_funcionario
                (id, tenant_id, funcionario_id, beneficio_catalogo_id, vigencia_inicio, created_at)
            VALUES (UUID(), @t, @f, @cat, '2026-06-01', UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@t"] = tenantId,
                ["@f"] = funcionarioId,
                ["@cat"] = Guid.NewGuid().ToString() // catalogo inexistente
            });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_NO_REFERENCED_ROW_2);
        ex.Message.Should().Contain("fk_beneficios_func_cat");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado dependente apontando para funcionario_id inexistente, quando insere, então MySqlException(1452) cita fk_dependentes_func")]
    public async Task Dependente_FuncionarioIdInexistente_RejeitadaComFkCitada()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);

        var act = () => db.ExecuteAsync(@"
            INSERT INTO dependentes
                (id, tenant_id, funcionario_id, nome_completo, data_nascimento, tipo, created_at)
            VALUES (UUID(), @t, @f, 'Filho Fantasma', '2018-01-01', 'Filho', UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@t"] = tenantId,
                ["@f"] = Guid.NewGuid().ToString()
            });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_NO_REFERENCED_ROW_2);
        ex.Message.Should().Contain("fk_dependentes_func");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado escala_funcionario apontando para jornada_id inexistente, quando insere, então MySqlException(1452) cita fk_escalas_func_jornada")]
    public async Task EscalaFuncionario_JornadaIdInexistente_RejeitadaComFkCitada()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        var funcionarioId = await InserirFuncionarioComMatricula(db, tenantId, "ESC-001", cpf: "55555555555");

        var act = () => db.ExecuteAsync(@"
            INSERT INTO escalas_funcionario
                (id, tenant_id, funcionario_id, jornada_id, vigencia_inicio, created_at)
            VALUES (UUID(), @t, @f, @j, '2026-06-01', UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@t"] = tenantId,
                ["@f"] = funcionarioId,
                ["@j"] = Guid.NewGuid().ToString()
            });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_NO_REFERENCED_ROW_2);
        ex.Message.Should().Contain("fk_escalas_func_jornada");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado historico_salario apontando para funcionario_id inexistente, quando insere, então MySqlException(1452) cita fk_historico_salarios_func")]
    public async Task HistoricoSalario_FuncionarioIdInexistente_RejeitadaComFkCitada()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);

        var act = () => db.ExecuteAsync(@"
            INSERT INTO historico_salarios
                (id, tenant_id, funcionario_id, valor, vigencia_inicio, motivo, created_at)
            VALUES (UUID(), @t, @f, 3000.00, '2026-06-01', 'Admissao', UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@t"] = tenantId,
                ["@f"] = Guid.NewGuid().ToString()
            });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_NO_REFERENCED_ROW_2);
        ex.Message.Should().Contain("fk_historico_salarios_func");
    }

    // ----------------------------------------------- JSON nativo / NOT NULL

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado jornada com janelas_json textualmente inválido, quando insere, então MySqlException(3140) cita 'Invalid JSON'")]
    public async Task Jornada_JanelasJsonInvalido_RejeitadaPelaValidacaoJsonNativa()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);

        var act = () => db.ExecuteAsync(@"
            INSERT INTO jornadas
                (id, tenant_id, nome, tipo, carga_semanal_horas, janelas_json, created_at)
            VALUES (UUID(), @t, '44h CLT', 'Fixa', 44.00, '{ isto nao eh json', UTC_TIMESTAMP())",
            new Dictionary<string, object?> { ["@t"] = tenantId });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_INVALID_JSON_TEXT);
        ex.Message.Should().Contain("Invalid JSON", "MySQL retorna mensagem em inglês independente do locale");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado jornada sem janelas_json (NULL), quando insere, então MySqlException(1048) cita 'cannot be null' e janelas_json")]
    public async Task Jornada_JanelasJsonNull_RejeitadaPorNotNull()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);

        var act = () => db.ExecuteAsync(@"
            INSERT INTO jornadas
                (id, tenant_id, nome, tipo, carga_semanal_horas, janelas_json, created_at)
            VALUES (UUID(), @t, '44h CLT', 'Fixa', 44.00, NULL, UTC_TIMESTAMP())",
            new Dictionary<string, object?> { ["@t"] = tenantId });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_BAD_NULL_ERROR);
        ex.Message.Should().Contain("janelas_json").And.Contain("cannot be null");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "ConstraintsRhFundacao")]
    [SkippableFact(DisplayName = "Dado historico_salario sem motivo (NULL), quando insere, então MySqlException(1048) cita 'motivo' e 'cannot be null'")]
    public async Task HistoricoSalario_MotivoNull_RejeitadaPorNotNull()
    {
        Skip.IfNot(Docker.IsAvailable, $"Docker indisponível: {Docker.UnavailableReason}");

        _ = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var tenantId = await SeedTenantTeste(db);
        var funcionarioId = await InserirFuncionarioComMatricula(db, tenantId, "HS-001", cpf: "66666666666");

        var act = () => db.ExecuteAsync(@"
            INSERT INTO historico_salarios
                (id, tenant_id, funcionario_id, valor, vigencia_inicio, motivo, created_at)
            VALUES (UUID(), @t, @f, 3000.00, '2026-06-01', NULL, UTC_TIMESTAMP())",
            new Dictionary<string, object?> { ["@t"] = tenantId, ["@f"] = funcionarioId });

        var ex = (await act.Should().ThrowAsync<MySqlException>()).Which;
        ex.Number.Should().Be(ER_BAD_NULL_ERROR);
        ex.Message.Should().Contain("motivo").And.Contain("cannot be null");
    }

    // ---------------------------------------------------------------------- Helpers

    private static async Task<string> SeedTenantTeste(IDataConfiguration db)
    {
        var tenantId = Guid.NewGuid().ToString();
        var cnpj = Guid.NewGuid().ToString("N")[..14];
        await db.ExecuteAsync(@"
            INSERT INTO tenants (id, razao_social, cnpj, plano, status, created_at)
            VALUES (@id, @razao, @cnpj, 'FREE', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = tenantId,
                ["@razao"] = "Tenant Erro " + cnpj[..6],
                ["@cnpj"] = cnpj
            });
        return tenantId;
    }

    private static async Task<string> InserirFuncionarioComMatricula(
        IDataConfiguration db, string tenantId, string matricula, string cpf)
    {
        var id = Guid.NewGuid().ToString();
        await db.ExecuteAsync(@"
            INSERT INTO funcionarios
                (id, tenant_id, nome_completo, cpf, codigo_matricula, status, created_at)
            VALUES (@id, @t, CONCAT('Funcionario ', @mat), @cpf, @mat, 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@t"] = tenantId,
                ["@mat"] = matricula,
                ["@cpf"] = cpf
            });
        return id;
    }

    private static async Task InserirCargo(IDataConfiguration db, string tenantId, string codigo, string descricao)
    {
        await db.ExecuteAsync(@"
            INSERT INTO cargos (id, tenant_id, codigo, descricao, ativo, created_at)
            VALUES (UUID(), @t, @c, @d, 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@t"] = tenantId,
                ["@c"] = codigo,
                ["@d"] = descricao
            });
    }

    private static async Task InserirBeneficioCatalogo(IDataConfiguration db, string tenantId, string codigo, string descricao)
    {
        await db.ExecuteAsync(@"
            INSERT INTO beneficios_catalogo
                (id, tenant_id, codigo, descricao, tipo, ativo, created_at)
            VALUES (UUID(), @t, @c, @d, 'ValeTransporte', 1, UTC_TIMESTAMP())",
            new Dictionary<string, object?>
            {
                ["@t"] = tenantId,
                ["@c"] = codigo,
                ["@d"] = descricao
            });
    }
}
