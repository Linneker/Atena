using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260629005_AddTabelaMovimentosBancoHoras : IMigration
{
    public long Version => 20260629005;
    public string Name => "AddTabelaMovimentosBancoHoras";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS movimentos_banco_horas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                data DATE NOT NULL,
                origem VARCHAR(20) NOT NULL,
                minutos INT NOT NULL,
                referencia_marcacao_id CHAR(36) NULL,
                competencia CHAR(7) NOT NULL,
                observacao VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_mov_bh_tenant (tenant_id),
                INDEX ix_mov_bh_func_compet (tenant_id, funcionario_id, competencia),
                INDEX ix_mov_bh_data (tenant_id, funcionario_id, data),
                CONSTRAINT fk_mov_bh_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_mov_bh_funcionario FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id),
                CONSTRAINT fk_mov_bh_marcacao FOREIGN KEY (referencia_marcacao_id) REFERENCES marcacoes_ponto(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS movimentos_banco_horas;");
}
