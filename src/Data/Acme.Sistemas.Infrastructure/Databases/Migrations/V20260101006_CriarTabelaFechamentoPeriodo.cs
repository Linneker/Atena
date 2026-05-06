using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101006_CriarTabelaFechamentoPeriodo : IMigration
{
    public long Version => 20260101006;
    public string Name => "CriarTabelaFechamentoPeriodo";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS fechamento_periodos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                ano SMALLINT NOT NULL,
                mes TINYINT NOT NULL,
                fechado_em DATETIME NOT NULL,
                fechado_por CHAR(36) NULL,
                total_receitas DECIMAL(15,2) NOT NULL DEFAULT 0,
                total_despesas DECIMAL(15,2) NOT NULL DEFAULT 0,
                resultado DECIMAL(15,2) NOT NULL DEFAULT 0,
                observacao VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_fechamento_tenant_periodo (tenant_id, ano, mes),
                INDEX ix_fechamento_tenant (tenant_id),
                CONSTRAINT fk_fechamento_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS fechamento_periodos;");
    }
}
