using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Numerador atômico de NSR (Número Sequencial de Registro) da Portaria MTP 671/2021.
/// Mesma mecânica do <c>nfe_numeracao</c>: INSERT … ON DUPLICATE KEY UPDATE col = LAST_INSERT_ID(col+1).
/// Único por (tenant, empresa).
/// </summary>
public sealed class V20260629001_AddTabelaNumeradorNsr : IMigration
{
    public long Version => 20260629001;
    public string Name => "AddTabelaNumeradorNsr";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS numerador_nsr (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                empresa_id CHAR(36) NOT NULL,
                ultimo_numero BIGINT NOT NULL DEFAULT 0,
                atualizado_em DATETIME(6) NOT NULL,
                UNIQUE KEY uk_numerador_nsr_tenant_empresa (tenant_id, empresa_id),
                INDEX idx_numerador_nsr_tenant (tenant_id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
        ");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS numerador_nsr;");
}
