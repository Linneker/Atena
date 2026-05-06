using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101005_CriarTabelasDespesaReceita : IMigration
{
    public long Version => 20260101005;
    public string Name => "CriarTabelasDespesaReceita";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS despesas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                descricao TEXT NULL,
                categoria VARCHAR(100) NULL,
                valor DECIMAL(15,2) NOT NULL,
                despesa_fixa TINYINT NOT NULL DEFAULT 0,
                data_vencimento DATETIME NOT NULL,
                competencia_id CHAR(36) NULL,
                centro_de_custo_id CHAR(36) NULL,
                fornecedor_id CHAR(36) NULL,
                status_pagamento TINYINT NOT NULL DEFAULT 0,
                valor_pago DECIMAL(15,2) NULL,
                data_pagamento DATETIME NULL,
                forma_pagamento TINYINT NULL,
                observacao_pagamento VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_despesas_tenant (tenant_id),
                INDEX ix_despesas_vencimento (tenant_id, data_vencimento),
                INDEX ix_despesas_status (tenant_id, status_pagamento),
                INDEX ix_despesas_categoria (tenant_id, categoria),
                CONSTRAINT fk_despesas_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS receitas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                descricao TEXT NULL,
                categoria VARCHAR(100) NULL,
                valor DECIMAL(15,2) NOT NULL,
                receita_fixa TINYINT NOT NULL DEFAULT 0,
                data_prevista_recebimento DATETIME NOT NULL,
                competencia_id CHAR(36) NULL,
                centro_de_custo_id CHAR(36) NULL,
                cliente_id CHAR(36) NULL,
                origem_venda_id CHAR(36) NULL,
                status_recebimento TINYINT NOT NULL DEFAULT 0,
                valor_recebido DECIMAL(15,2) NULL,
                data_recebimento DATETIME NULL,
                forma_pagamento TINYINT NULL,
                observacao_recebimento VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_receitas_tenant (tenant_id),
                INDEX ix_receitas_data (tenant_id, data_prevista_recebimento),
                INDEX ix_receitas_status (tenant_id, status_recebimento),
                CONSTRAINT fk_receitas_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS despesas;");
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS receitas;");
    }
}
