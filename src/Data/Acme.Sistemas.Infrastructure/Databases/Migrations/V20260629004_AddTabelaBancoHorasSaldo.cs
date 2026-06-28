using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260629004_AddTabelaBancoHorasSaldo : IMigration
{
    public long Version => 20260629004;
    public string Name => "AddTabelaBancoHorasSaldo";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS banco_horas_saldo (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                competencia CHAR(7) NOT NULL,
                horas_devidas DECIMAL(8,2) NOT NULL DEFAULT 0,
                horas_realizadas DECIMAL(8,2) NOT NULL DEFAULT 0,
                saldo_minutos INT NOT NULL DEFAULT 0,
                politica_id CHAR(36) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                INDEX ix_bh_saldo_tenant (tenant_id),
                INDEX ix_bh_saldo_func_compet (tenant_id, funcionario_id, competencia),
                UNIQUE KEY ux_bh_saldo_func_compet (tenant_id, funcionario_id, competencia),
                CONSTRAINT fk_bh_saldo_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_bh_saldo_funcionario FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id),
                CONSTRAINT fk_bh_saldo_politica FOREIGN KEY (politica_id) REFERENCES politicas_banco_horas(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS banco_horas_saldo;");
}
