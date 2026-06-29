using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Tabela IRRF — faixas progressivas mensais com parcela a deduzir, vigentes por competência.
/// Inclui o valor de dedução por dependente. Catálogo nacional. Seed inline traz valores
/// exemplares 2026 (ajustar via upload admin quando RFB publicar valores oficiais).
/// </summary>
public sealed class V20260701002_AddTabelaIrrf : IMigration
{
    public long Version => 20260701002;
    public string Name => "AddTabelaIrrf";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tabela_irrf (
                id CHAR(36) NOT NULL PRIMARY KEY,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                ordem_faixa TINYINT NOT NULL,
                faixa_inicio DECIMAL(12,2) NOT NULL,
                faixa_fim DECIMAL(12,2) NOT NULL,
                aliquota_pct DECIMAL(5,2) NOT NULL,
                parcela_deduzir DECIMAL(10,2) NOT NULL DEFAULT 0,
                deducao_por_dependente DECIMAL(10,2) NOT NULL DEFAULT 0,
                deducao_simplificada DECIMAL(10,2) NOT NULL DEFAULT 0,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                UNIQUE KEY uk_tabela_irrf_comp_ordem (competencia_inicio, ordem_faixa),
                INDEX ix_tabela_irrf_vigencia (competencia_inicio, competencia_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        // IRRF mensal 2026 (valores exemplares pós-simplificação Lei 14.848/2024)
        // Dedução por dependente: R$189,59; dedução simplificada: R$564,80
        var faixas = new (int Ordem, decimal Ini, decimal Fim, decimal Aliq, decimal Parc)[]
        {
            (1, 0.00m,    2428.80m,  0.0m,  0.00m),
            (2, 2428.81m, 2826.65m,  7.5m,  182.16m),
            (3, 2826.66m, 3751.05m,  15.0m, 394.16m),
            (4, 3751.06m, 4664.68m,  22.5m, 675.49m),
            (5, 4664.69m, 999999.99m, 27.5m, 908.73m),
        };

        foreach (var f in faixas)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO tabela_irrf
                    (id, competencia_inicio, competencia_fim, ordem_faixa, faixa_inicio, faixa_fim,
                     aliquota_pct, parcela_deduzir, deducao_por_dependente, deducao_simplificada,
                     seed_origem, importado_em)
                VALUES
                    (UUID(), '2026-01', NULL, @ordem, @ini, @fim, @aliq, @parc, 189.59, 564.80, 'migration', UTC_TIMESTAMP(6))
                ON DUPLICATE KEY UPDATE
                    faixa_inicio = VALUES(faixa_inicio),
                    faixa_fim = VALUES(faixa_fim),
                    aliquota_pct = VALUES(aliquota_pct),
                    parcela_deduzir = VALUES(parcela_deduzir);";
            AddParam(cmd, "@ordem", f.Ordem);
            AddParam(cmd, "@ini", f.Ini);
            AddParam(cmd, "@fim", f.Fim);
            AddParam(cmd, "@aliq", f.Aliq);
            AddParam(cmd, "@parc", f.Parc);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tabela_irrf;");

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        cmd.Parameters.Add(p);
    }
}
