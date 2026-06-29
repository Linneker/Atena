using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Comprovante de marcação Portaria 671/2021 anexo II: payload texto fixo +
/// assinatura ICP-Brasil A1/A3 (RSA-SHA-256) + hash SHA-256 do payload.
/// FK 1:1 com <c>marcacoes_ponto</c>; NSR único por empresa.
/// </summary>
public sealed class V20260629002_AddTabelaComprovantesPonto : IMigration
{
    public long Version => 20260629002;
    public string Name => "AddTabelaComprovantesPonto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS comprovantes_ponto (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                empresa_id CHAR(36) NOT NULL,
                marcacao_id CHAR(36) NOT NULL,
                nsr BIGINT NOT NULL,
                payload_texto TEXT NOT NULL,
                assinatura_base64 TEXT NOT NULL,
                hash_sha256 CHAR(64) NOT NULL,
                certificado_thumbprint CHAR(64) NULL,
                emitido_em DATETIME(6) NOT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                UNIQUE KEY uk_comprovantes_marcacao (tenant_id, marcacao_id),
                UNIQUE KEY uk_comprovantes_empresa_nsr (tenant_id, empresa_id, nsr),
                INDEX idx_comprovantes_tenant (tenant_id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS comprovantes_ponto;");
}
