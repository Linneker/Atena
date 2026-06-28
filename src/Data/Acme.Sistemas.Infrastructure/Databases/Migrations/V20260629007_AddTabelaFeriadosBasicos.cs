using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Subset nacional de feriados (~14). Catálogo opt-in completo (estaduais/municipais)
/// virá em W5 via upload admin. Tabela tenant-scoped para permitir feriados próprios.
/// </summary>
public sealed class V20260629007_AddTabelaFeriadosBasicos : IMigration
{
    public long Version => 20260629007;
    public string Name => "AddTabelaFeriadosBasicos";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS feriados (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NULL,
                data DATE NOT NULL,
                descricao VARCHAR(200) NOT NULL,
                tipo VARCHAR(20) NOT NULL DEFAULT 'Nacional',
                uf CHAR(2) NULL,
                municipio_ibge VARCHAR(7) NULL,
                ativo TINYINT(1) NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                INDEX ix_feriados_data (data),
                INDEX ix_feriados_tenant (tenant_id, data),
                CONSTRAINT fk_feriados_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        // Seed dos 14 feriados nacionais 2026 (subset; ano-base de demo).
        var feriados = new (string Data, string Desc)[]
        {
            ("2026-01-01", "Confraternização Universal"),
            ("2026-02-16", "Carnaval (segunda)"),
            ("2026-02-17", "Carnaval (terça)"),
            ("2026-02-18", "Quarta-feira de Cinzas (meio expediente)"),
            ("2026-04-03", "Sexta-feira Santa"),
            ("2026-04-21", "Tiradentes"),
            ("2026-05-01", "Dia do Trabalho"),
            ("2026-06-04", "Corpus Christi"),
            ("2026-09-07", "Independência do Brasil"),
            ("2026-10-12", "Nossa Senhora Aparecida"),
            ("2026-11-02", "Finados"),
            ("2026-11-15", "Proclamação da República"),
            ("2026-11-20", "Dia da Consciência Negra"),
            ("2026-12-25", "Natal"),
        };

        foreach (var (data, desc) in feriados)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO feriados (id, tenant_id, data, descricao, tipo, ativo, created_at)
                VALUES (UUID(), NULL, @d, @desc, 'Nacional', 1, UTC_TIMESTAMP())
                ON DUPLICATE KEY UPDATE descricao = VALUES(descricao);";
            var pd = cmd.CreateParameter(); pd.ParameterName = "@d"; pd.Value = data; cmd.Parameters.Add(pd);
            var pdesc = cmd.CreateParameter(); pdesc.ParameterName = "@desc"; pdesc.Value = desc; cmd.Parameters.Add(pdesc);
            cmd.ExecuteNonQuery();
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS feriados;");
}
