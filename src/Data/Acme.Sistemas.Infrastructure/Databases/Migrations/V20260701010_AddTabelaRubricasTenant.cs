using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Rubricas customizáveis por tenant. Cada tenant define suas rubricas em DSL (W5 Fase 4).
/// Tenant pode clonar do <c>rubricas_catalogo_nacional</c> ou criar do zero.
/// CRUD vai em W5 Fase 5; folha consome em W6.
/// </summary>
public sealed class V20260701010_AddTabelaRubricasTenant : IMigration
{
    public long Version => 20260701010;
    public string Name => "AddTabelaRubricasTenant";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS rubricas_tenant (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                codigo VARCHAR(30) NOT NULL,
                descricao VARCHAR(200) NOT NULL,
                tipo VARCHAR(20) NOT NULL,
                natureza_esocial_codigo VARCHAR(10) NULL,
                formula_dsl TEXT NOT NULL,
                incide_inss TINYINT(1) NOT NULL DEFAULT 0,
                incide_irrf TINYINT(1) NOT NULL DEFAULT 0,
                incide_fgts TINYINT(1) NOT NULL DEFAULT 0,
                incide_ferias TINYINT(1) NOT NULL DEFAULT 0,
                incide_13o TINYINT(1) NOT NULL DEFAULT 0,
                incide_dsr TINYINT(1) NOT NULL DEFAULT 0,
                dependencias_json JSON NULL,
                vigencia_inicio DATE NOT NULL,
                vigencia_fim DATE NULL,
                ativa TINYINT(1) NOT NULL DEFAULT 1,
                origem VARCHAR(20) NOT NULL DEFAULT 'Custom',
                codigo_catalogo_origem VARCHAR(30) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY uk_rubricas_tenant_cod (tenant_id, codigo),
                INDEX ix_rubricas_tenant_vigencia (tenant_id, ativa, vigencia_inicio, vigencia_fim),
                CONSTRAINT fk_rubricas_tenant_tenant FOREIGN KEY (tenant_id)
                    REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_rubricas_tenant_natureza FOREIGN KEY (natureza_esocial_codigo)
                    REFERENCES naturezas_rubrica_esocial(codigo) ON DELETE SET NULL
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS rubricas_tenant;");
}
