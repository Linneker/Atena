using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Tabela INSS — faixas vigentes por competência. Modelo pós-Reforma Previdência 2019 (4 faixas
/// progressivas com parcela a deduzir). Catálogo nacional (não tenant-scoped). Vigências fechadas
/// via <c>competencia_fim</c>; NULL = vigente. Seed inline traz valores exemplares 2026.
/// </summary>
public sealed class V20260701001_AddTabelaInss : IMigration
{
    public long Version => 20260701001;
    public string Name => "AddTabelaInss";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tabela_inss (
                id CHAR(36) NOT NULL PRIMARY KEY,
                competencia_inicio CHAR(7) NOT NULL,
                competencia_fim CHAR(7) NULL,
                ordem_faixa TINYINT NOT NULL,
                faixa_inicio DECIMAL(12,2) NOT NULL,
                faixa_fim DECIMAL(12,2) NOT NULL,
                aliquota_pct DECIMAL(5,2) NOT NULL,
                parcela_deduzir DECIMAL(10,2) NOT NULL DEFAULT 0,
                seed_origem VARCHAR(20) NOT NULL DEFAULT 'migration',
                importado_em DATETIME(6) NOT NULL,
                importado_por CHAR(36) NULL,
                UNIQUE KEY uk_tabela_inss_comp_ordem (competencia_inicio, ordem_faixa),
                INDEX ix_tabela_inss_vigencia (competencia_inicio, competencia_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        var faixas = new (int Ordem, decimal Ini, decimal Fim, decimal Aliq, decimal Parc)[]
        {
            (1, 0.00m,    1518.00m, 7.5m,  0.00m),
            (2, 1518.01m, 2793.88m, 9.0m,  22.77m),
            (3, 2793.89m, 4190.83m, 12.0m, 106.59m),
            (4, 4190.84m, 8157.41m, 14.0m, 190.40m),
        };

        foreach (var f in faixas)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO tabela_inss
                    (id, competencia_inicio, competencia_fim, ordem_faixa, faixa_inicio, faixa_fim,
                     aliquota_pct, parcela_deduzir, seed_origem, importado_em)
                VALUES
                    (UUID(), '2026-01', NULL, @ordem, @ini, @fim, @aliq, @parc, 'migration', UTC_TIMESTAMP(6))
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
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tabela_inss;");

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value;
        cmd.Parameters.Add(p);
    }
}
