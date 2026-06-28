using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Solicitações de ajuste de ponto com workflow de aprovação. Aprovado vira nova
/// MarcacaoPonto com status=Ajustada, preservando cadeia de hash e auditoria.
/// </summary>
public sealed class V20260629002_AddTabelaAjustesPonto : IMigration
{
    public long Version => 20260629002;
    public string Name => "AddTabelaAjustesPonto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS ajustes_ponto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                marcacao_original_id CHAR(36) NULL,
                tipo_ajuste VARCHAR(30) NOT NULL,
                data_hora_proposta DATETIME(0) NULL,
                tipo_marcacao_proposta VARCHAR(20) NULL,
                motivo TEXT NOT NULL,
                anexo_url VARCHAR(500) NULL,
                status VARCHAR(20) NOT NULL DEFAULT 'Pendente',
                aprovador_id CHAR(36) NULL,
                decisao_em DATETIME NULL,
                justificativa_decisao TEXT NULL,
                marcacao_resultante_id CHAR(36) NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_ajustes_tenant (tenant_id),
                INDEX ix_ajustes_func (tenant_id, funcionario_id, status),
                INDEX ix_ajustes_pendentes (tenant_id, status, created_at),
                CONSTRAINT fk_ajustes_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_ajustes_funcionario FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id),
                CONSTRAINT fk_ajustes_marcacao FOREIGN KEY (marcacao_original_id) REFERENCES marcacoes_ponto(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS ajustes_ponto;");
}
