using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101018_CriarTabelasAuditoria : IMigration
{
    public long Version => 20260101018;
    public string Name => "CriarTabelasAuditoria";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS audit_logs (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                user_id CHAR(36) NULL,
                entidade_nome VARCHAR(100) NOT NULL,
                entidade_id CHAR(36) NULL,
                operacao TINYINT NOT NULL,
                command_tipo VARCHAR(255) NOT NULL,
                antes_json MEDIUMTEXT NULL,
                depois_json MEDIUMTEXT NULL,
                ocorrido_em DATETIME NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_audit_tenant (tenant_id, ocorrido_em),
                INDEX ix_audit_entidade (tenant_id, entidade_nome, entidade_id),
                INDEX ix_audit_user (tenant_id, user_id, ocorrido_em),
                CONSTRAINT fk_audit_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS api_request_audit (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                user_id CHAR(36) NULL,
                metodo VARCHAR(10) NOT NULL,
                caminho VARCHAR(500) NOT NULL,
                query_string VARCHAR(2000) NULL,
                status_code INT NOT NULL,
                duracao_ms BIGINT NOT NULL,
                ip_address VARCHAR(45) NULL,
                user_agent VARCHAR(500) NULL,
                correlation_id VARCHAR(64) NULL,
                ocorrido_em DATETIME NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_apiaudit_tenant (tenant_id, ocorrido_em),
                INDEX ix_apiaudit_user (tenant_id, user_id, ocorrido_em),
                INDEX ix_apiaudit_corr (correlation_id),
                CONSTRAINT fk_apiaudit_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS audit_logs;");
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS api_request_audit;");
    }
}
