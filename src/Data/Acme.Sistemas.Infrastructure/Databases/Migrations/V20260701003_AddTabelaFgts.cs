using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Tabela FGTS — alíquotas vigentes por competência. Normal 8%, multa rescisória 40%,
/// contribuição social (LC 110/2001, suspensa desde 2020) mantida como coluna zerável para auditoria.
/// </summary>
public sealed class V20260701003_AddTabelaFgts : IMigration
{
    public long Version => 20260701003;
    public string Name => "AddTabelaFgts";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tabela_fgts (
                id CHAR(36) NOT NULL PRIMARY KEY,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                aliquota_normal_pct DECIMAL(5,2) NOT NULL,
                aliquota_multa_rescisao_pct DECIMAL(5,2) NOT NULL,
                aliquota_contribuicao_social_pct DECIMAL(5,2) NOT NULL DEFAULT 0,
                aliquota_aprendiz_pct DECIMAL(5,2) NOT NULL DEFAULT 2,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                UNIQUE KEY uk_tabela_fgts_comp (competencia_inicio),
                INDEX ix_tabela_fgts_vigencia (competencia_inicio, competencia_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            INSERT INTO tabela_fgts
                (id, competencia_inicio, competencia_fim, aliquota_normal_pct, aliquota_multa_rescisao_pct,
                 aliquota_contribuicao_social_pct, aliquota_aprendiz_pct, seed_origem, importado_em)
            VALUES
                (UUID(), '2026-01', NULL, 8.00, 40.00, 0.00, 2.00, 'migration', UTC_TIMESTAMP(6))
            ON DUPLICATE KEY UPDATE
                aliquota_normal_pct = VALUES(aliquota_normal_pct),
                aliquota_multa_rescisao_pct = VALUES(aliquota_multa_rescisao_pct);");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tabela_fgts;");
}
