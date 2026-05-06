using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101013_CriarTabelasEstoque : IMigration
{
    public long Version => 20260101013;
    public string Name => "CriarTabelasEstoque";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS estoques (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                codigo VARCHAR(30) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                localizacao VARCHAR(255) NULL,
                permite_saldo_negativo TINYINT NOT NULL DEFAULT 0,
                ativo TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_estoques_tenant_codigo (tenant_id, codigo),
                INDEX ix_estoques_tenant (tenant_id),
                CONSTRAINT fk_estoques_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS estoque_produtos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                estoque_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                saldo_total DECIMAL(15,4) NOT NULL DEFAULT 0,
                saldo_reservado DECIMAL(15,4) NOT NULL DEFAULT 0,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_estoque_produto (tenant_id, estoque_id, produto_id),
                INDEX ix_estoque_produto_tenant (tenant_id),
                INDEX ix_estoque_produto_produto (tenant_id, produto_id),
                CONSTRAINT fk_estoque_produtos_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_estoque_produtos_estoque FOREIGN KEY (estoque_id) REFERENCES estoques(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS entrada_produto_estoque (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                estoque_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                custo_unitario DECIMAL(15,4) NULL,
                origem TINYINT NOT NULL DEFAULT 0,
                motivo VARCHAR(500) NULL,
                fornecedor_id CHAR(36) NULL,
                documento_referencia VARCHAR(100) NULL,
                data_movimento DATETIME NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_entrada_tenant (tenant_id),
                INDEX ix_entrada_produto (tenant_id, produto_id, data_movimento),
                INDEX ix_entrada_estoque (tenant_id, estoque_id, data_movimento),
                CONSTRAINT fk_entrada_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS saida_produto_estoque (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                estoque_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                quantidade DECIMAL(15,4) NOT NULL,
                custo_unitario DECIMAL(15,4) NULL,
                origem TINYINT NOT NULL DEFAULT 0,
                motivo VARCHAR(500) NULL,
                cliente_id CHAR(36) NULL,
                documento_referencia VARCHAR(100) NULL,
                data_movimento DATETIME NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_saida_tenant (tenant_id),
                INDEX ix_saida_produto (tenant_id, produto_id, data_movimento),
                INDEX ix_saida_estoque (tenant_id, estoque_id, data_movimento),
                CONSTRAINT fk_saida_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS inventarios (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                estoque_id CHAR(36) NOT NULL,
                data_abertura DATETIME NOT NULL,
                data_fechamento DATETIME NULL,
                status TINYINT NOT NULL DEFAULT 0,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_inventarios_tenant (tenant_id),
                INDEX ix_inventarios_estoque (tenant_id, estoque_id, status),
                CONSTRAINT fk_inventarios_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_inventarios_estoque FOREIGN KEY (estoque_id) REFERENCES estoques(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS inventario_itens (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                inventario_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                saldo_sistema DECIMAL(15,4) NOT NULL,
                saldo_contado DECIMAL(15,4) NULL,
                observacao VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_inv_itens_tenant (tenant_id),
                INDEX ix_inv_itens_inventario (tenant_id, inventario_id),
                CONSTRAINT fk_inv_itens_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_inv_itens_inv FOREIGN KEY (inventario_id) REFERENCES inventarios(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var t in new[] { "inventario_itens", "inventarios",
            "saida_produto_estoque", "entrada_produto_estoque",
            "estoque_produtos", "estoques" })
        {
            MigrationHelper.Execute(connection, transaction, $"DROP TABLE IF EXISTS {t};");
        }
    }
}
