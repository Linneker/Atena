using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101012_CriarTabelasProdutos : IMigration
{
    public long Version => 20260101012;
    public string Name => "CriarTabelasProdutos";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tipos_produto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                descricao VARCHAR(2000) NULL,
                ativo TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_tipos_produto_tenant (tenant_id),
                CONSTRAINT fk_tipos_produto_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS tipos_valor_produto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                descricao VARCHAR(2000) NULL,
                ativo TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_tipos_valor_tenant (tenant_id),
                CONSTRAINT fk_tipos_valor_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS produtos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                codigo VARCHAR(50) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                descricao VARCHAR(2000) NULL,
                codigo_barras VARCHAR(64) NULL,
                unidade_medida VARCHAR(10) NOT NULL DEFAULT 'UN',
                tipo_produto_id CHAR(36) NULL,
                fornecedor_id CHAR(36) NULL,
                custo_medio DECIMAL(15,4) NULL,
                estoque_minimo DECIMAL(15,4) NULL,
                status TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_produtos_tenant_codigo (tenant_id, codigo),
                INDEX ix_produtos_tenant (tenant_id),
                INDEX ix_produtos_barras (tenant_id, codigo_barras),
                CONSTRAINT fk_produtos_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS valores_produto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                produto_id CHAR(36) NOT NULL,
                tipo_valor_produto_id CHAR(36) NOT NULL,
                valor DECIMAL(15,4) NOT NULL,
                vigencia_inicio DATETIME NOT NULL,
                vigencia_fim DATETIME NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_valores_produto (tenant_id, produto_id),
                INDEX ix_valores_tipo (tenant_id, tipo_valor_produto_id),
                CONSTRAINT fk_valores_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_valores_produto FOREIGN KEY (produto_id) REFERENCES produtos(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var t in new[] { "valores_produto", "produtos", "tipos_valor_produto", "tipos_produto" })
            MigrationHelper.Execute(connection, transaction, $"DROP TABLE IF EXISTS {t};");
    }
}
