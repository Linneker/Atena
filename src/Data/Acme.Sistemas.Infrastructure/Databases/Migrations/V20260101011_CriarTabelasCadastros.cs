using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101011_CriarTabelasCadastros : IMigration
{
    public long Version => 20260101011;
    public string Name => "CriarTabelasCadastros";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS clientes (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                tipo TINYINT NOT NULL DEFAULT 2,
                nome VARCHAR(255) NOT NULL,
                nome_fantasia VARCHAR(255) NULL,
                documento VARCHAR(20) NOT NULL,
                inscricao_estadual VARCHAR(50) NULL,
                email VARCHAR(255) NULL,
                telefone VARCHAR(30) NULL,
                status TINYINT NOT NULL DEFAULT 1,
                inadimplente TINYINT NOT NULL DEFAULT 0,
                bloqueado_vendas TINYINT NOT NULL DEFAULT 0,
                endereco_cep VARCHAR(10) NULL,
                endereco_logradouro VARCHAR(255) NULL,
                endereco_numero VARCHAR(20) NULL,
                endereco_complemento VARCHAR(100) NULL,
                endereco_bairro VARCHAR(100) NULL,
                endereco_cidade VARCHAR(100) NULL,
                endereco_uf CHAR(2) NULL,
                endereco_pais VARCHAR(50) NULL DEFAULT 'BR',
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_clientes_tenant_doc (tenant_id, documento),
                INDEX ix_clientes_tenant (tenant_id),
                INDEX ix_clientes_inadimplente (tenant_id, inadimplente),
                CONSTRAINT fk_clientes_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS fornecedores (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                tipo TINYINT NOT NULL DEFAULT 2,
                nome VARCHAR(255) NOT NULL,
                nome_fantasia VARCHAR(255) NULL,
                documento VARCHAR(20) NOT NULL,
                inscricao_estadual VARCHAR(50) NULL,
                email VARCHAR(255) NULL,
                telefone VARCHAR(30) NULL,
                condicao_pagamento_padrao VARCHAR(100) NULL,
                status TINYINT NOT NULL DEFAULT 1,
                endereco_cep VARCHAR(10) NULL,
                endereco_logradouro VARCHAR(255) NULL,
                endereco_numero VARCHAR(20) NULL,
                endereco_complemento VARCHAR(100) NULL,
                endereco_bairro VARCHAR(100) NULL,
                endereco_cidade VARCHAR(100) NULL,
                endereco_uf CHAR(2) NULL,
                endereco_pais VARCHAR(50) NULL DEFAULT 'BR',
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_fornecedores_tenant_doc (tenant_id, documento),
                INDEX ix_fornecedores_tenant (tenant_id),
                CONSTRAINT fk_fornecedores_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");

        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS funcionarios (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                nome_completo VARCHAR(255) NOT NULL,
                cpf VARCHAR(20) NOT NULL,
                email VARCHAR(255) NULL,
                telefone VARCHAR(30) NULL,
                cargo VARCHAR(100) NULL,
                departamento VARCHAR(100) NULL,
                centro_de_custo_id CHAR(36) NULL,
                data_admissao DATE NULL,
                data_demissao DATE NULL,
                usuario_id CHAR(36) NULL,
                status TINYINT NOT NULL DEFAULT 1,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY ux_funcionarios_tenant_cpf (tenant_id, cpf),
                INDEX ix_funcionarios_tenant (tenant_id),
                CONSTRAINT fk_funcionarios_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        foreach (var table in new[] { "funcionarios", "fornecedores", "clientes" })
        {
            MigrationHelper.Execute(connection, transaction, $"DROP TABLE IF EXISTS {table};");
        }
    }
}
