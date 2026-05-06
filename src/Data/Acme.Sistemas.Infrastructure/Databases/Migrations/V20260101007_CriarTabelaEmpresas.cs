using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101007_CriarTabelaEmpresas : IMigration
{
    public long Version => 20260101007;
    public string Name => "CriarTabelaEmpresas";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS empresas (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                razao_social VARCHAR(255) NOT NULL,
                nome_fantasia VARCHAR(255) NULL,
                cnpj VARCHAR(20) NOT NULL,
                inscricao_estadual VARCHAR(50) NULL,
                inscricao_municipal VARCHAR(50) NULL,
                email VARCHAR(255) NULL,
                telefone VARCHAR(30) NULL,
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
                UNIQUE KEY ux_empresas_tenant_cnpj (tenant_id, cnpj),
                INDEX ix_empresas_tenant (tenant_id),
                CONSTRAINT fk_empresas_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS empresas;");
    }
}
