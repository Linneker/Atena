using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Dispositivos móveis registrados (rh-mobile-maui W3). Cada funcionário pode ter múltiplos
/// dispositivos (celular + tablet). Push tokens FCM/APNs persistidos aqui. Admin pode revogar.
/// </summary>
public sealed class V20260630001_AddTabelaDispositivosMobile : IMigration
{
    public long Version => 20260630001;
    public string Name => "AddTabelaDispositivosMobile";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS dispositivos_mobile (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NULL,
                usuario_id CHAR(36) NOT NULL,
                device_id VARCHAR(120) NOT NULL,
                plataforma VARCHAR(20) NOT NULL,
                modelo VARCHAR(120) NULL,
                os_version VARCHAR(40) NULL,
                app_version VARCHAR(20) NULL,
                push_token VARCHAR(500) NULL,
                chave_publica_local TEXT NULL,
                ativo TINYINT(1) NOT NULL DEFAULT 1,
                revogado_em DATETIME NULL,
                revogado_por CHAR(36) NULL,
                registrado_em DATETIME NOT NULL,
                ultimo_acesso DATETIME NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                INDEX ix_disp_tenant (tenant_id),
                INDEX ix_disp_user (tenant_id, usuario_id, ativo),
                UNIQUE KEY ux_disp_tenant_user_device (tenant_id, usuario_id, device_id),
                CONSTRAINT fk_disp_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_disp_usuario FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
                CONSTRAINT fk_disp_funcionario FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS dispositivos_mobile;");
}
