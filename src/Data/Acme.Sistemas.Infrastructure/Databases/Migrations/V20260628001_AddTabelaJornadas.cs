using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260628001_AddTabelaJornadas : IMigration
{
    public long Version => 20260628001;
    public string Name => "AddTabelaJornadas";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS jornadas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(80) NOT NULL,
                tipo VARCHAR(30) NOT NULL,
                carga_semanal_horas DECIMAL(5,2) NOT NULL,
                carga_diaria_horas DECIMAL(5,2) NULL,
                janelas_json JSON NOT NULL,
                permite_marcar_intervalo TINYINT(1) NOT NULL DEFAULT 1,
                tolerancia_minutos INT NOT NULL DEFAULT 10,
                ativo TINYINT(1) NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_jornadas_tenant (tenant_id),
                INDEX ix_jornadas_tenant_ativo (tenant_id, ativo),
                CONSTRAINT fk_jornadas_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS jornadas;");
}
