using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260510001_CriarTabelaNFeNumeracao : IMigration
{
    public long Version => 20260510001;
    public string Name => "CriarTabelaNFeNumeracao";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS nfe_numeracao (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                cnpj VARCHAR(14) NOT NULL,
                serie INT NOT NULL,
                ultimo_numero BIGINT NOT NULL DEFAULT 0,
                atualizado_em DATETIME NOT NULL,
                UNIQUE KEY uk_nfe_numeracao_tenant_cnpj_serie (tenant_id, cnpj, serie),
                INDEX idx_nfe_numeracao_tenant (tenant_id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, "DROP TABLE IF EXISTS nfe_numeracao;");
    }

    private static void Exec(IDbConnection conn, IDbTransaction tx, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
