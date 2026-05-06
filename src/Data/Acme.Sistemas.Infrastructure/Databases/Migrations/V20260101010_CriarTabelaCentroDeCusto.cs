using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101010_CriarTabelaCentroDeCusto : IMigration
{
    public long Version => 20260101010;
    public string Name => "CriarTabelaCentroDeCusto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS centros_de_custo (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                codigo VARCHAR(30) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                descricao VARCHAR(2000) NULL,
                responsavel_id CHAR(36) NULL,
                ativo TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_centros_tenant_codigo (tenant_id, codigo),
                INDEX ix_centros_tenant (tenant_id),
                CONSTRAINT fk_centros_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS centros_de_custo;");
    }
}
