using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Salário-mínimo nacional vigente por competência. Catálogo nacional (não tenant-scoped).
/// Seed traz valor exemplar 2026 (ajustar via upload quando MP confirmar).
/// </summary>
public sealed class V20260701004_AddTabelaSalarioMinimoNacional : IMigration
{
    public long Version => 20260701004;
    public string Name => "AddTabelaSalarioMinimoNacional";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS salario_minimo_nacional (
                id CHAR(36) NOT NULL PRIMARY KEY,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                valor DECIMAL(10,2) NOT NULL,
                ato_legal VARCHAR(200) NULL,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                UNIQUE KEY uk_sm_nac_comp (competencia_inicio),
                INDEX ix_sm_nac_vigencia (competencia_inicio, competencia_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO salario_minimo_nacional
                (id, competencia_inicio, competencia_fim, valor, ato_legal, seed_origem, importado_em)
            VALUES
                (UUID(), '2026-01', NULL, 1518.00, 'Valor exemplar 2026 (ajustar quando MP oficial publicada)', 'migration', UTC_TIMESTAMP(6))
            ON DUPLICATE KEY UPDATE valor = VALUES(valor);");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS salario_minimo_nacional;");
}
