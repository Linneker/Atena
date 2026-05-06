using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101001_CriarTabelasTenant : IMigration
{
    public long Version => 20260101001;
    public string Name => "CriarTabelasTenant";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tenants (
                id CHAR(36) NOT NULL PRIMARY KEY,
                razao_social VARCHAR(255) NOT NULL,
                cnpj VARCHAR(18) NOT NULL,
                plano VARCHAR(20) NOT NULL DEFAULT 'FREE',
                status TINYINT NOT NULL DEFAULT 1,
                logo_url VARCHAR(500) NULL,
                cor_primaria VARCHAR(20) NULL,
                fuso_horario VARCHAR(50) NOT NULL DEFAULT 'America/Sao_Paulo',
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_tenants_cnpj (cnpj),
                INDEX ix_tenants_status (status)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tenant_limites (
                tenant_id CHAR(36) NOT NULL PRIMARY KEY,
                max_usuarios INT NOT NULL DEFAULT 5,
                max_nfe_mes INT NOT NULL DEFAULT 100,
                max_storage_gb INT NOT NULL DEFAULT 1,
                updated_at DATETIME NOT NULL,
                CONSTRAINT fk_tenant_limites_tenant
                    FOREIGN KEY (tenant_id) REFERENCES tenants(id)
                    ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tenant_limites;");
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS tenants;");
    }
}
