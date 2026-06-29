using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Regra do vale-transporte. Lei 7.418/85 + Decreto 95.247/87 — desconto máximo de 6% do
/// salário-base do funcionário. Persistido para auditoria e para o engine de folha (W6) consultar
/// sem hardcode. Vigência por competência.
/// </summary>
public sealed class V20260701007_AddTabelaValeTransporte : IMigration
{
    public long Version => 20260701007;
    public string Name => "AddTabelaValeTransporte";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tabela_vale_transporte (
                id CHAR(36) NOT NULL PRIMARY KEY,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                desconto_max_pct DECIMAL(5,2) NOT NULL,
                ato_legal VARCHAR(200) NULL,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                UNIQUE KEY uk_vt_comp (competencia_inicio)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO tabela_vale_transporte
                (id, competencia_inicio, competencia_fim, desconto_max_pct, ato_legal, seed_origem, importado_em)
            VALUES
                (UUID(), '2026-01', NULL, 6.00, 'Lei 7.418/85; Decreto 95.247/87 (art. 9)', 'migration', UTC_TIMESTAMP(6))
            ON DUPLICATE KEY UPDATE desconto_max_pct = VALUES(desconto_max_pct);");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tabela_vale_transporte;");
}
