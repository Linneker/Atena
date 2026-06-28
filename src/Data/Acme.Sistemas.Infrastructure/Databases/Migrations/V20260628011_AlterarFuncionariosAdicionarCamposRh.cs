using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Adiciona à tabela <c>funcionarios</c> as colunas exigidas por RH/Folha/eSocial
/// (cargo_id, lotacao_id, departamento_id, dados pessoais, PIS, CTPS, RG, endereço e
/// conta bancária em JSON). Cada ADD COLUMN é guardado por <see cref="MigrationHelper.ColumnExists"/>
/// para tornar a migration idempotente. A unique key de matrícula é criada por último.
/// </summary>
public sealed class V20260628011_AlterarFuncionariosAdicionarCamposRh : IMigration
{
    public long Version => 20260628011;
    public string Name => "AlterarFuncionariosAdicionarCamposRh";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        AddColumnIfMissing(connection, transaction, "cargo_id", "ADD COLUMN cargo_id CHAR(36) NULL");
        AddColumnIfMissing(connection, transaction, "lotacao_id", "ADD COLUMN lotacao_id CHAR(36) NULL");
        AddColumnIfMissing(connection, transaction, "departamento_id", "ADD COLUMN departamento_id CHAR(36) NULL");
        AddColumnIfMissing(connection, transaction, "tipo_contrato", "ADD COLUMN tipo_contrato VARCHAR(40) NULL");
        AddColumnIfMissing(connection, transaction, "regime_remuneracao", "ADD COLUMN regime_remuneracao VARCHAR(30) NULL");
        AddColumnIfMissing(connection, transaction, "codigo_matricula", "ADD COLUMN codigo_matricula VARCHAR(30) NULL");
        AddColumnIfMissing(connection, transaction, "pis", "ADD COLUMN pis CHAR(11) NULL");
        AddColumnIfMissing(connection, transaction, "ctps", "ADD COLUMN ctps VARCHAR(20) NULL");
        AddColumnIfMissing(connection, transaction, "ctps_serie", "ADD COLUMN ctps_serie VARCHAR(10) NULL");
        AddColumnIfMissing(connection, transaction, "ctps_uf", "ADD COLUMN ctps_uf CHAR(2) NULL");
        AddColumnIfMissing(connection, transaction, "rg", "ADD COLUMN rg VARCHAR(20) NULL");
        AddColumnIfMissing(connection, transaction, "rg_orgao", "ADD COLUMN rg_orgao VARCHAR(20) NULL");
        AddColumnIfMissing(connection, transaction, "rg_uf", "ADD COLUMN rg_uf CHAR(2) NULL");
        AddColumnIfMissing(connection, transaction, "estado_civil", "ADD COLUMN estado_civil VARCHAR(20) NULL");
        AddColumnIfMissing(connection, transaction, "naturalidade", "ADD COLUMN naturalidade VARCHAR(80) NULL");
        AddColumnIfMissing(connection, transaction, "nacionalidade", "ADD COLUMN nacionalidade VARCHAR(40) NULL DEFAULT 'Brasileira'");
        AddColumnIfMissing(connection, transaction, "endereco_json", "ADD COLUMN endereco_json JSON NULL");
        AddColumnIfMissing(connection, transaction, "conta_bancaria_json", "ADD COLUMN conta_bancaria_json JSON NULL");

        if (!IndexExists(connection, transaction, "funcionarios", "ux_funcionarios_tenant_matricula"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE funcionarios ADD UNIQUE KEY ux_funcionarios_tenant_matricula (tenant_id, codigo_matricula);");
        }

        if (!IndexExists(connection, transaction, "funcionarios", "ix_funcionarios_cargo"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE funcionarios ADD INDEX ix_funcionarios_cargo (tenant_id, cargo_id);");
        }

        if (!IndexExists(connection, transaction, "funcionarios", "ix_funcionarios_departamento"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE funcionarios ADD INDEX ix_funcionarios_departamento (tenant_id, departamento_id);");
        }

        if (!IndexExists(connection, transaction, "funcionarios", "ix_funcionarios_lotacao"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE funcionarios ADD INDEX ix_funcionarios_lotacao (tenant_id, lotacao_id);");
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var idx in new[] { "ux_funcionarios_tenant_matricula", "ix_funcionarios_cargo", "ix_funcionarios_departamento", "ix_funcionarios_lotacao" })
        {
            if (IndexExists(connection, transaction, "funcionarios", idx))
            {
                MigrationHelper.Execute(connection, transaction, $"ALTER TABLE funcionarios DROP INDEX {idx};");
            }
        }

        foreach (var col in new[]
        {
            "cargo_id","lotacao_id","departamento_id","tipo_contrato","regime_remuneracao","codigo_matricula",
            "pis","ctps","ctps_serie","ctps_uf","rg","rg_orgao","rg_uf","estado_civil","naturalidade",
            "nacionalidade","endereco_json","conta_bancaria_json"
        })
        {
            if (MigrationHelper.ColumnExists(connection, transaction, "funcionarios", col))
            {
                MigrationHelper.Execute(connection, transaction, $"ALTER TABLE funcionarios DROP COLUMN {col};");
            }
        }
    }

    private static void AddColumnIfMissing(IDbConnection conn, IDbTransaction tx, string column, string addClause)
    {
        if (!MigrationHelper.ColumnExists(conn, tx, "funcionarios", column))
        {
            MigrationHelper.Execute(conn, tx, $"ALTER TABLE funcionarios {addClause};");
        }
    }

    private static bool IndexExists(IDbConnection conn, IDbTransaction tx, string table, string indexName)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = @"SELECT COUNT(*) FROM information_schema.statistics
                            WHERE table_schema = DATABASE() AND table_name = @t AND index_name = @i";
        var t = cmd.CreateParameter(); t.ParameterName = "@t"; t.Value = table; cmd.Parameters.Add(t);
        var i = cmd.CreateParameter(); i.ParameterName = "@i"; i.Value = indexName; cmd.Parameters.Add(i);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }
}
