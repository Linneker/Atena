using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260628004_AddTabelaDepartamentos : IMigration
{
    public long Version => 20260628004;
    public string Name => "AddTabelaDepartamentos";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS departamentos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                codigo VARCHAR(20) NULL,
                nome VARCHAR(120) NOT NULL,
                centro_de_custo_id CHAR(36) NULL,
                ativo TINYINT(1) NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_departamentos_tenant (tenant_id),
                INDEX ix_departamentos_tenant_ativo (tenant_id, ativo),
                UNIQUE KEY ux_departamentos_tenant_codigo (tenant_id, codigo),
                CONSTRAINT fk_departamentos_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS departamentos;");
}
