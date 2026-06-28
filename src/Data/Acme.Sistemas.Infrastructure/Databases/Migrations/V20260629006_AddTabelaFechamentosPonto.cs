using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260629006_AddTabelaFechamentosPonto : IMigration
{
    public long Version => 20260629006;
    public string Name => "AddTabelaFechamentosPonto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS fechamentos_ponto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                competencia CHAR(7) NOT NULL,
                status VARCHAR(20) NOT NULL DEFAULT 'Aberto',
                fechado_em DATETIME NULL,
                fechado_por CHAR(36) NULL,
                reaberto_em DATETIME NULL,
                reaberto_por CHAR(36) NULL,
                motivo_reabertura TEXT NULL,
                espelho_url VARCHAR(500) NULL,
                espelho_hash CHAR(64) NULL,
                observacoes TEXT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                INDEX ix_fech_ponto_tenant (tenant_id),
                INDEX ix_fech_ponto_compet (tenant_id, competencia, status),
                UNIQUE KEY ux_fech_ponto_func_compet (tenant_id, funcionario_id, competencia),
                CONSTRAINT fk_fech_ponto_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_fech_ponto_funcionario FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS fechamentos_ponto;");
}
