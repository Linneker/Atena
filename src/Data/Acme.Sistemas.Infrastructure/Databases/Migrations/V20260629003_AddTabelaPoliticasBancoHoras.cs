using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260629003_AddTabelaPoliticasBancoHoras : IMigration
{
    public long Version => 20260629003;
    public string Name => "AddTabelaPoliticasBancoHoras";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS politicas_banco_horas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(120) NOT NULL,
                vigencia_inicio DATE NOT NULL,
                vigencia_fim DATE NULL,
                limite_horas_acumular DECIMAL(8,2) NOT NULL DEFAULT 40,
                prazo_compensacao_dias INT NOT NULL DEFAULT 180,
                permite_pagar_excedente TINYINT(1) NOT NULL DEFAULT 1,
                fator_pagamento DECIMAL(4,2) NOT NULL DEFAULT 1.00,
                ativo TINYINT(1) NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_politicas_bh_tenant (tenant_id),
                UNIQUE KEY ux_politicas_bh_tenant_nome (tenant_id, nome),
                CONSTRAINT fk_politicas_bh_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS politicas_banco_horas;");
}
