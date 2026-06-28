using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Marcações de ponto do funcionário com hash-chain de integridade (SHA-256 encadeado).
/// Adulteração de qualquer linha quebra o hash de todas as seguintes — detectado por
/// <c>JobVerificarIntegridadePonto</c>. Hash não substitui ICP-Brasil (W4).
/// </summary>
public sealed class V20260629001_AddTabelaMarcacoesPonto : IMigration
{
    public long Version => 20260629001;
    public string Name => "AddTabelaMarcacoesPonto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS marcacoes_ponto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                tipo VARCHAR(20) NOT NULL,
                data_hora DATETIME(0) NOT NULL,
                origem VARCHAR(20) NOT NULL,
                latitude DECIMAL(10,7) NULL,
                longitude DECIMAL(10,7) NULL,
                ip_origem VARCHAR(45) NULL,
                user_agent VARCHAR(255) NULL,
                device_id VARCHAR(100) NULL,
                foto_url VARCHAR(500) NULL,
                hash_anterior CHAR(64) NULL,
                hash_integridade CHAR(64) NOT NULL,
                status VARCHAR(20) NOT NULL DEFAULT 'Valida',
                marcacao_origem_id CHAR(36) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_marcacoes_tenant (tenant_id),
                INDEX ix_marcacoes_func_data (tenant_id, funcionario_id, data_hora),
                INDEX ix_marcacoes_status (tenant_id, status),
                CONSTRAINT fk_marcacoes_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_marcacoes_funcionario FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS marcacoes_ponto;");
}
