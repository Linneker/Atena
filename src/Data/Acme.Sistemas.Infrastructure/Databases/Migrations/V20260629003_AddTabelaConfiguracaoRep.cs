using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Configuração do REP (Registrador Eletrônico de Ponto) por empresa do tenant.
/// Tipo P (programa) ou C (cloud); REP-A (hardware) fora de escopo.
/// Certificado é o mesmo já gerido pelo <c>CertificadoTenantResolver</c>.
/// </summary>
public sealed class V20260629003_AddTabelaConfiguracaoRep : IMigration
{
    public long Version => 20260629003;
    public string Name => "AddTabelaConfiguracaoRep";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS configuracao_rep (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                empresa_id CHAR(36) NOT NULL,
                tipo VARCHAR(10) NOT NULL,
                razao_social VARCHAR(150) NOT NULL,
                cnpj_cei VARCHAR(14) NOT NULL,
                cno VARCHAR(20) NULL,
                inscricao_estadual VARCHAR(20) NULL,
                cnae_principal VARCHAR(10) NULL,
                endereco_logradouro VARCHAR(150) NOT NULL,
                endereco_numero VARCHAR(20) NULL,
                endereco_complemento VARCHAR(50) NULL,
                endereco_bairro VARCHAR(80) NULL,
                endereco_cidade VARCHAR(80) NOT NULL,
                endereco_uf CHAR(2) NOT NULL,
                endereco_cep VARCHAR(10) NULL,
                certificado_id CHAR(36) NOT NULL,
                responsavel_cpf VARCHAR(11) NOT NULL,
                responsavel_nome VARCHAR(150) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY uk_configuracao_rep_empresa (tenant_id, empresa_id),
                INDEX idx_configuracao_rep_tenant (tenant_id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS configuracao_rep;");
}
