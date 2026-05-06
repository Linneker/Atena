using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101003_CriarTabelasRbac : IMigration
{
    public long Version => 20260101003;
    public string Name => "CriarTabelasRbac";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS roles (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(100) NOT NULL,
                descricao VARCHAR(500) NULL,
                is_system TINYINT NOT NULL DEFAULT 0,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_roles_tenant_nome (tenant_id, nome),
                INDEX ix_roles_tenant (tenant_id),
                CONSTRAINT fk_roles_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS permissions (
                id CHAR(36) NOT NULL PRIMARY KEY,
                recurso VARCHAR(50) NOT NULL,
                acao VARCHAR(20) NOT NULL,
                codigo VARCHAR(80) NOT NULL,
                descricao VARCHAR(255) NULL,
                UNIQUE KEY ux_permissions_codigo (codigo),
                INDEX ix_permissions_recurso (recurso)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS role_permissions (
                role_id CHAR(36) NOT NULL,
                permission_id CHAR(36) NOT NULL,
                granted_at DATETIME NOT NULL,
                granted_by CHAR(36) NULL,
                PRIMARY KEY (role_id, permission_id),
                CONSTRAINT fk_role_permissions_role FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE CASCADE,
                CONSTRAINT fk_role_permissions_permission FOREIGN KEY (permission_id) REFERENCES permissions(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS user_roles (
                user_id CHAR(36) NOT NULL,
                role_id CHAR(36) NOT NULL,
                tenant_id CHAR(36) NOT NULL,
                granted_at DATETIME NOT NULL,
                granted_by CHAR(36) NULL,
                expires_at DATETIME NULL,
                PRIMARY KEY (user_id, role_id),
                INDEX ix_user_roles_tenant (tenant_id),
                CONSTRAINT fk_user_roles_role FOREIGN KEY (role_id) REFERENCES roles(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS api_keys (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(100) NOT NULL,
                key_hash CHAR(128) NOT NULL,
                permissions_json TEXT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                expires_at DATETIME NULL,
                revoked_at DATETIME NULL,
                last_used_at DATETIME NULL,
                UNIQUE KEY ux_api_keys_hash (key_hash),
                INDEX ix_api_keys_tenant (tenant_id),
                CONSTRAINT fk_api_keys_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS refresh_tokens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                user_id CHAR(36) NOT NULL,
                token_hash CHAR(128) NOT NULL,
                jti CHAR(36) NOT NULL,
                issued_at DATETIME NOT NULL,
                expires_at DATETIME NOT NULL,
                revoked_at DATETIME NULL,
                replaced_by CHAR(36) NULL,
                user_agent VARCHAR(500) NULL,
                ip_address VARCHAR(50) NULL,
                UNIQUE KEY ux_refresh_tokens_hash (token_hash),
                INDEX ix_refresh_tokens_user (user_id),
                INDEX ix_refresh_tokens_jti (jti)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS token_blacklist (
                jti CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                user_id CHAR(36) NULL,
                blacklisted_at DATETIME NOT NULL,
                expires_at DATETIME NOT NULL,
                reason VARCHAR(255) NULL,
                INDEX ix_token_blacklist_expires (expires_at)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in new[] { "token_blacklist", "refresh_tokens", "api_keys",
                                       "user_roles", "role_permissions", "permissions", "roles" })
        {
            MigrationHelper.Execute(connection, transaction, $"DROP TABLE IF EXISTS {table};");
        }
    }
}
