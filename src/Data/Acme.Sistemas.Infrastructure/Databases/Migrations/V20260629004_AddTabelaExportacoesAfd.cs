using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Catálogo das exportações AFD (Arquivo Fonte de Dados) — Portaria 671 anexo I.
/// Arquivo binário fica em S3/GED; aqui ficam metadados + hash p/ verificação.
/// </summary>
public sealed class V20260629004_AddTabelaExportacoesAfd : IMigration
{
    public long Version => 20260629004;
    public string Name => "AddTabelaExportacoesAfd";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS exportacoes_afd (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                empresa_id CHAR(36) NOT NULL,
                periodo_inicio DATE NOT NULL,
                periodo_fim DATE NOT NULL,
                layout_versao VARCHAR(10) NOT NULL DEFAULT '003',
                arquivo_url VARCHAR(500) NULL,
                hash_sha256 CHAR(64) NULL,
                status VARCHAR(20) NOT NULL DEFAULT 'Solicitada',
                gerado_em DATETIME(6) NULL,
                erro TEXT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                INDEX idx_exportacoes_afd_tenant (tenant_id),
                INDEX idx_exportacoes_afd_empresa_periodo (tenant_id, empresa_id, periodo_inicio, periodo_fim)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS exportacoes_afd;");
}
