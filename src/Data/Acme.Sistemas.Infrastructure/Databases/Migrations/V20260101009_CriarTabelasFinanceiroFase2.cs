using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101009_CriarTabelasFinanceiroFase2 : IMigration
{
    public long Version => 20260101009;
    public string Name => "CriarTabelasFinanceiroFase2";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS plano_de_contas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                codigo VARCHAR(30) NOT NULL,
                nome VARCHAR(255) NOT NULL,
                tipo TINYINT NOT NULL,
                pai_id CHAR(36) NULL,
                nivel INT NOT NULL DEFAULT 1,
                aceita_lancamento TINYINT NOT NULL DEFAULT 1,
                ativo TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_plano_tenant_codigo (tenant_id, codigo),
                INDEX ix_plano_tenant (tenant_id),
                INDEX ix_plano_pai (tenant_id, pai_id),
                CONSTRAINT fk_plano_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS dividas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                credor VARCHAR(255) NOT NULL,
                descricao VARCHAR(2000) NULL,
                valor_original DECIMAL(15,2) NOT NULL,
                valor_pago DECIMAL(15,2) NOT NULL DEFAULT 0,
                taxa_juros_mensal DECIMAL(10,4) NULL,
                data_inicio DATE NOT NULL,
                data_fim DATE NULL,
                numero_parcelas INT NOT NULL DEFAULT 1,
                status TINYINT NOT NULL DEFAULT 0,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_dividas_tenant (tenant_id),
                INDEX ix_dividas_status (tenant_id, status),
                CONSTRAINT fk_dividas_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS contas_pagar (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                descricao VARCHAR(500) NOT NULL,
                fornecedor_id CHAR(36) NULL,
                despesa_id CHAR(36) NULL,
                plano_de_contas_id CHAR(36) NULL,
                valor_original DECIMAL(15,2) NOT NULL,
                valor_pago DECIMAL(15,2) NOT NULL DEFAULT 0,
                data_vencimento DATE NOT NULL,
                data_pagamento DATETIME NULL,
                status TINYINT NOT NULL DEFAULT 0,
                observacao VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_contas_pagar_tenant (tenant_id),
                INDEX ix_contas_pagar_vencimento (tenant_id, data_vencimento),
                INDEX ix_contas_pagar_status (tenant_id, status),
                CONSTRAINT fk_contas_pagar_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS contas_receber (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                descricao VARCHAR(500) NOT NULL,
                cliente_id CHAR(36) NULL,
                receita_id CHAR(36) NULL,
                plano_de_contas_id CHAR(36) NULL,
                valor_original DECIMAL(15,2) NOT NULL,
                valor_recebido DECIMAL(15,2) NOT NULL DEFAULT 0,
                data_vencimento DATE NOT NULL,
                data_recebimento DATETIME NULL,
                status TINYINT NOT NULL DEFAULT 0,
                observacao_recebimento VARCHAR(2000) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_contas_receber_tenant (tenant_id),
                INDEX ix_contas_receber_vencimento (tenant_id, data_vencimento),
                INDEX ix_contas_receber_status (tenant_id, status),
                INDEX ix_contas_receber_cliente (tenant_id, cliente_id),
                CONSTRAINT fk_contas_receber_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS pagamentos (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                despesa_id CHAR(36) NULL,
                divida_id CHAR(36) NULL,
                conta_pagar_id CHAR(36) NULL,
                valor DECIMAL(15,2) NOT NULL,
                data_pagamento DATETIME NOT NULL,
                forma_pagamento TINYINT NOT NULL,
                observacao VARCHAR(500) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_pagamentos_tenant (tenant_id),
                INDEX ix_pagamentos_conta_pagar (tenant_id, conta_pagar_id),
                INDEX ix_pagamentos_divida (tenant_id, divida_id),
                CONSTRAINT fk_pagamentos_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS conciliacoes_bancarias (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                banco VARCHAR(100) NOT NULL,
                agencia VARCHAR(20) NULL,
                conta VARCHAR(30) NULL,
                periodo_inicio DATE NOT NULL,
                periodo_fim DATE NOT NULL,
                formato_arquivo VARCHAR(10) NOT NULL DEFAULT 'CSV',
                status TINYINT NOT NULL DEFAULT 0,
                total_lancamentos INT NOT NULL DEFAULT 0,
                total_conciliados INT NOT NULL DEFAULT 0,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_conciliacoes_tenant (tenant_id),
                CONSTRAINT fk_conciliacoes_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS itens_extrato (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                conciliacao_id CHAR(36) NOT NULL,
                data_movimento DATE NOT NULL,
                valor DECIMAL(15,2) NOT NULL,
                tipo TINYINT NOT NULL,
                descricao VARCHAR(500) NULL,
                documento_bancario VARCHAR(100) NULL,
                status TINYINT NOT NULL DEFAULT 0,
                conta_pagar_id CHAR(36) NULL,
                conta_receber_id CHAR(36) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_itens_extrato_tenant (tenant_id),
                INDEX ix_itens_extrato_conciliacao (tenant_id, conciliacao_id),
                INDEX ix_itens_extrato_status (tenant_id, status),
                CONSTRAINT fk_itens_extrato_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_itens_extrato_conciliacao FOREIGN KEY (conciliacao_id) REFERENCES conciliacoes_bancarias(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in new[] { "itens_extrato", "conciliacoes_bancarias", "pagamentos", "contas_receber", "contas_pagar", "dividas", "plano_de_contas" })
        {
            MigrationHelper.Execute(connection, transaction, $"DROP TABLE IF EXISTS {table};");
        }
    }
}
