using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Salário-família — limite de remuneração + valor da cota por dependente.
/// Versionada por competência. Seed inicial 2026 (valores exemplares, ajustar via upload).
/// </summary>
public sealed class V20260701006_AddTabelaSalarioFamilia : IMigration
{
    public long Version => 20260701006;
    public string Name => "AddTabelaSalarioFamilia";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tabela_salario_familia (
                id CHAR(36) NOT NULL PRIMARY KEY,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                limite_remuneracao DECIMAL(10,2) NOT NULL,
                valor_cota DECIMAL(10,2) NOT NULL,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                UNIQUE KEY uk_sf_comp (competencia_inicio),
                INDEX ix_sf_vigencia (competencia_inicio, competencia_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO tabela_salario_familia
                (id, competencia_inicio, competencia_fim, limite_remuneracao, valor_cota, seed_origem, importado_em)
            VALUES
                (UUID(), '2026-01', NULL, 1819.26, 65.00, 'migration', UTC_TIMESTAMP(6))
            ON DUPLICATE KEY UPDATE
                limite_remuneracao = VALUES(limite_remuneracao),
                valor_cota = VALUES(valor_cota);");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tabela_salario_familia;");
}
