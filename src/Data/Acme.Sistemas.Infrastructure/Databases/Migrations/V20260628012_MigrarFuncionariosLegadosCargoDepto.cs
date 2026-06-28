using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Backfill que converte os campos texto <c>funcionarios.cargo</c> / <c>departamento</c>
/// em FKs para as novas tabelas <c>cargos</c> / <c>departamentos</c>, e atribui lotação
/// "Sede" default a cada tenant. Funcionários sem cargo/depto/lotação recebem entidade
/// "Não classificado" criada na hora. Nenhum dado é perdido.
/// </summary>
public sealed class V20260628012_MigrarFuncionariosLegadosCargoDepto : IMigration
{
    public long Version => 20260628012;
    public string Name => "MigrarFuncionariosLegadosCargoDepto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        // 1) Garantir "Não classificado" por tenant em cargos e departamentos, e "Sede" em lotações.
        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO cargos (id, tenant_id, codigo, descricao, ativo, created_at)
            SELECT UUID(), t.id, 'NAO-CLASS', 'Não classificado', 1, UTC_TIMESTAMP()
            FROM tenants t
            WHERE NOT EXISTS (
                SELECT 1 FROM cargos c WHERE c.tenant_id = t.id AND c.codigo = 'NAO-CLASS'
            );");

        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO departamentos (id, tenant_id, codigo, nome, ativo, created_at)
            SELECT UUID(), t.id, 'NAO-CLASS', 'Não classificado', 1, UTC_TIMESTAMP()
            FROM tenants t
            WHERE NOT EXISTS (
                SELECT 1 FROM departamentos d WHERE d.tenant_id = t.id AND d.codigo = 'NAO-CLASS'
            );");

        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO lotacoes (id, tenant_id, nome, ativo, created_at)
            SELECT UUID(), t.id, 'Sede', 1, UTC_TIMESTAMP()
            FROM tenants t
            WHERE NOT EXISTS (
                SELECT 1 FROM lotacoes l WHERE l.tenant_id = t.id AND l.nome = 'Sede'
            );");

        // 2) Criar cargos a partir de cargo (texto) distinto por tenant que ainda não existem como descrição.
        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO cargos (id, tenant_id, codigo, descricao, ativo, created_at)
            SELECT UUID(), f.tenant_id, NULL, f.cargo, 1, UTC_TIMESTAMP()
            FROM (
                SELECT DISTINCT tenant_id, cargo
                FROM funcionarios
                WHERE cargo IS NOT NULL AND TRIM(cargo) <> ''
            ) f
            WHERE NOT EXISTS (
                SELECT 1 FROM cargos c WHERE c.tenant_id = f.tenant_id AND c.descricao = f.cargo
            );");

        // 3) Backfill cargo_id em funcionarios.
        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios f
            JOIN cargos c ON c.tenant_id = f.tenant_id AND c.descricao = f.cargo
            SET f.cargo_id = c.id
            WHERE f.cargo IS NOT NULL AND TRIM(f.cargo) <> '' AND f.cargo_id IS NULL;");

        // 4) Para funcionários sem cargo, atribuir 'Não classificado' do tenant.
        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios f
            JOIN cargos c ON c.tenant_id = f.tenant_id AND c.codigo = 'NAO-CLASS'
            SET f.cargo_id = c.id
            WHERE f.cargo_id IS NULL;");

        // 5) Mesma sequência para departamentos.
        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO departamentos (id, tenant_id, codigo, nome, ativo, created_at)
            SELECT UUID(), f.tenant_id, NULL, f.departamento, 1, UTC_TIMESTAMP()
            FROM (
                SELECT DISTINCT tenant_id, departamento
                FROM funcionarios
                WHERE departamento IS NOT NULL AND TRIM(departamento) <> ''
            ) f
            WHERE NOT EXISTS (
                SELECT 1 FROM departamentos d WHERE d.tenant_id = f.tenant_id AND d.nome = f.departamento
            );");

        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios f
            JOIN departamentos d ON d.tenant_id = f.tenant_id AND d.nome = f.departamento
            SET f.departamento_id = d.id
            WHERE f.departamento IS NOT NULL AND TRIM(f.departamento) <> '' AND f.departamento_id IS NULL;");

        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios f
            JOIN departamentos d ON d.tenant_id = f.tenant_id AND d.codigo = 'NAO-CLASS'
            SET f.departamento_id = d.id
            WHERE f.departamento_id IS NULL;");

        // 6) Atribuir lotação 'Sede' a todos sem lotação.
        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios f
            JOIN lotacoes l ON l.tenant_id = f.tenant_id AND l.nome = 'Sede'
            SET f.lotacao_id = l.id
            WHERE f.lotacao_id IS NULL;");

        // 7) Tipo de contrato e regime default = CLT/Mensalista para funcionários legados.
        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios
            SET tipo_contrato = 'Clt'
            WHERE tipo_contrato IS NULL;");

        MigrationHelper.Execute(connection, transaction, @"
            UPDATE funcionarios
            SET regime_remuneracao = 'Mensalista'
            WHERE regime_remuneracao IS NULL;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        // Backfill é não-destrutivo: os campos texto antigos continuam preenchidos.
        // Desfazer apenas zerar as FKs nos funcionários e remover entidades "Não classificado" / "Sede" auto-criadas.
        MigrationHelper.Execute(connection, transaction, "UPDATE funcionarios SET cargo_id = NULL, departamento_id = NULL, lotacao_id = NULL, tipo_contrato = NULL, regime_remuneracao = NULL;");
        MigrationHelper.Execute(connection, transaction, "DELETE FROM cargos WHERE codigo = 'NAO-CLASS';");
        MigrationHelper.Execute(connection, transaction, "DELETE FROM departamentos WHERE codigo = 'NAO-CLASS';");
        MigrationHelper.Execute(connection, transaction, "DELETE FROM lotacoes WHERE nome = 'Sede';");
    }
}
