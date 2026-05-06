using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101016_CriarTabelasVendas : IMigration
{
    public long Version => 20260101016;
    public string Name => "CriarTabelasVendas";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS orcamentos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                numero VARCHAR(30) NOT NULL,
                cliente_id CHAR(36) NOT NULL,
                vendedor_id CHAR(36) NULL,
                data_emissao DATETIME NOT NULL,
                data_validade DATETIME NOT NULL,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                desconto_percentual DECIMAL(5,2) NULL,
                status TINYINT NOT NULL DEFAULT 0,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_orc_tenant_numero (tenant_id, numero),
                INDEX ix_orc_tenant (tenant_id),
                INDEX ix_orc_status (tenant_id, status),
                INDEX ix_orc_cliente (tenant_id, cliente_id),
                CONSTRAINT fk_orc_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS orcamento_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                orcamento_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                preco_unitario DECIMAL(15,4) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_orc_item_tenant (tenant_id),
                INDEX ix_orc_item_orc (tenant_id, orcamento_id),
                CONSTRAINT fk_orc_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_orc_item_orc FOREIGN KEY (orcamento_id) REFERENCES orcamentos(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS pedidos_venda (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                numero VARCHAR(30) NOT NULL,
                cliente_id CHAR(36) NOT NULL,
                vendedor_id CHAR(36) NULL,
                orcamento_id CHAR(36) NULL,
                data_emissao DATETIME NOT NULL,
                estoque_id CHAR(36) NOT NULL,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                desconto_percentual DECIMAL(5,2) NULL,
                status TINYINT NOT NULL DEFAULT 0,
                condicao_pagamento VARCHAR(100) NULL,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_pv_tenant_numero (tenant_id, numero),
                INDEX ix_pv_tenant (tenant_id),
                INDEX ix_pv_status (tenant_id, status),
                INDEX ix_pv_cliente (tenant_id, cliente_id),
                INDEX ix_pv_vendedor (tenant_id, vendedor_id),
                CONSTRAINT fk_pv_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS pedido_venda_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                pedido_venda_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                quantidade_faturada DECIMAL(15,4) NOT NULL DEFAULT 0,
                preco_unitario DECIMAL(15,4) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_pv_item_tenant (tenant_id),
                INDEX ix_pv_item_ped (tenant_id, pedido_venda_id),
                CONSTRAINT fk_pv_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_pv_item_ped FOREIGN KEY (pedido_venda_id) REFERENCES pedidos_venda(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS faturamentos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                numero VARCHAR(30) NOT NULL,
                pedido_venda_id CHAR(36) NOT NULL,
                data_faturamento DATETIME NOT NULL,
                tipo TINYINT NOT NULL DEFAULT 1,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                nfe_id CHAR(36) NULL,
                conta_receber_id CHAR(36) NULL,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_fat_tenant_numero (tenant_id, numero),
                INDEX ix_fat_tenant (tenant_id),
                INDEX ix_fat_pedido (tenant_id, pedido_venda_id),
                CONSTRAINT fk_fat_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS faturamento_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                faturamento_id CHAR(36) NOT NULL,
                pedido_venda_item_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                preco_unitario DECIMAL(15,4) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_fat_item_tenant (tenant_id),
                INDEX ix_fat_item_fat (tenant_id, faturamento_id),
                CONSTRAINT fk_fat_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_fat_item_fat FOREIGN KEY (faturamento_id) REFERENCES faturamentos(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS devolucoes_venda (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                faturamento_id CHAR(36) NOT NULL,
                data_devolucao DATETIME NOT NULL,
                tipo TINYINT NOT NULL DEFAULT 1,
                valor_total DECIMAL(15,2) NOT NULL DEFAULT 0,
                motivo VARCHAR(2000) NULL,
                nfe_devolucao_id CHAR(36) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_dev_tenant (tenant_id),
                INDEX ix_dev_fat (tenant_id, faturamento_id),
                CONSTRAINT fk_dev_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS devolucao_venda_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                devolucao_venda_id CHAR(36) NOT NULL,
                faturamento_item_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                preco_unitario DECIMAL(15,4) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_dev_item_tenant (tenant_id),
                INDEX ix_dev_item_dev (tenant_id, devolucao_venda_id),
                CONSTRAINT fk_dev_item_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_dev_item_dev FOREIGN KEY (devolucao_venda_id) REFERENCES devolucoes_venda(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        Exec(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS comissoes_vendedor (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                vendedor_id CHAR(36) NOT NULL,
                faturamento_id CHAR(36) NOT NULL,
                base_calculo_valor DECIMAL(15,2) NOT NULL,
                percentual_comissao DECIMAL(5,2) NOT NULL,
                valor_comissao DECIMAL(15,2) NOT NULL,
                data_referencia DATETIME NOT NULL,
                status TINYINT NOT NULL DEFAULT 0,
                data_pagamento DATETIME NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_com_tenant (tenant_id),
                INDEX ix_com_vendedor (tenant_id, vendedor_id, status),
                INDEX ix_com_fat (tenant_id, faturamento_id),
                CONSTRAINT fk_com_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var t in new[] { "comissoes_vendedor",
            "devolucao_venda_itens", "devolucoes_venda",
            "faturamento_itens", "faturamentos",
            "pedido_venda_itens", "pedidos_venda",
            "orcamento_itens", "orcamentos" })
        {
            Exec(connection, transaction, $"DROP TABLE IF EXISTS {t};");
        }
    }

    private static void Exec(IDbConnection c, IDbTransaction t, string sql) =>
        MigrationHelper.Execute(c, t, sql);
}
