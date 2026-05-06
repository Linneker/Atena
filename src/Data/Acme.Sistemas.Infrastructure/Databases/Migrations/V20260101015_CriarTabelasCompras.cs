using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101015_CriarTabelasCompras : IMigration
{
    public long Version => 20260101015;
    public string Name => "CriarTabelasCompras";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS solicitacoes_compra (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                numero VARCHAR(30) NOT NULL,
                solicitante_id CHAR(36) NULL,
                justificativa VARCHAR(2000) NULL,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                data_solicitacao DATETIME NOT NULL,
                status TINYINT NOT NULL DEFAULT 0,
                aprovado_por CHAR(36) NULL,
                aprovado_em DATETIME NULL,
                motivo_rejeicao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_sol_compra_tenant_numero (tenant_id, numero),
                INDEX ix_sol_compra_tenant (tenant_id),
                INDEX ix_sol_compra_status (tenant_id, status),
                CONSTRAINT fk_sol_compra_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS solicitacao_compra_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                solicitacao_compra_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                preco_estimado DECIMAL(15,4) NULL,
                observacao VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_sol_item_tenant (tenant_id),
                INDEX ix_sol_item_sol (tenant_id, solicitacao_compra_id),
                CONSTRAINT fk_sol_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_sol_item_sol FOREIGN KEY (solicitacao_compra_id) REFERENCES solicitacoes_compra(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS pedidos_compra (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                numero VARCHAR(30) NOT NULL,
                fornecedor_id CHAR(36) NOT NULL,
                solicitacao_compra_id CHAR(36) NULL,
                data_emissao DATETIME NOT NULL,
                previsao_entrega DATE NULL,
                condicao_pagamento VARCHAR(100) NULL,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                status TINYINT NOT NULL DEFAULT 0,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_pedido_compra_tenant_numero (tenant_id, numero),
                INDEX ix_pedido_compra_tenant (tenant_id),
                INDEX ix_pedido_compra_status (tenant_id, status),
                INDEX ix_pedido_compra_fornecedor (tenant_id, fornecedor_id),
                CONSTRAINT fk_pedido_compra_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS pedido_compra_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                pedido_compra_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                quantidade_recebida DECIMAL(15,4) NOT NULL DEFAULT 0,
                preco_unitario DECIMAL(15,4) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_ped_item_tenant (tenant_id),
                INDEX ix_ped_item_pedido (tenant_id, pedido_compra_id),
                CONSTRAINT fk_ped_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_ped_item_pedido FOREIGN KEY (pedido_compra_id) REFERENCES pedidos_compra(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS recebimentos_compra (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                pedido_compra_id CHAR(36) NOT NULL,
                data_recebimento DATETIME NOT NULL,
                tipo TINYINT NOT NULL DEFAULT 1,
                numero_nota_fiscal VARCHAR(30) NULL,
                chave_acesso_nfe VARCHAR(50) NULL,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_receb_tenant (tenant_id),
                INDEX ix_receb_pedido (tenant_id, pedido_compra_id),
                CONSTRAINT fk_receb_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS recebimento_compra_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                recebimento_compra_id CHAR(36) NOT NULL,
                pedido_compra_item_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade_recebida DECIMAL(15,4) NOT NULL,
                preco_unitario DECIMAL(15,4) NULL,
                observacao VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_receb_item_tenant (tenant_id),
                INDEX ix_receb_item_receb (tenant_id, recebimento_compra_id),
                CONSTRAINT fk_receb_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_receb_item_receb FOREIGN KEY (recebimento_compra_id) REFERENCES recebimentos_compra(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var t in new[] { "recebimento_compra_itens", "recebimentos_compra",
            "pedido_compra_itens", "pedidos_compra",
            "solicitacao_compra_itens", "solicitacoes_compra" })
        {
            MigrationHelper.Execute(connection, transaction, $"DROP TABLE IF EXISTS {t};");
        }
    }
}
