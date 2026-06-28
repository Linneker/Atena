using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260628005_AddTabelaHistoricoSalarios : IMigration
{
    public long Version => 20260628005;
    public string Name => "AddTabelaHistoricoSalarios";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS historico_salarios (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                valor DECIMAL(10,2) NOT NULL,
                vigencia_inicio DATE NOT NULL,
                vigencia_fim DATE NULL,
                motivo VARCHAR(30) NOT NULL,
                observacao TEXT NULL,
                registrado_por_usuario_id CHAR(36) NULL,
                registrado_at DATETIME NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_historico_salarios_tenant (tenant_id),
                INDEX ix_historico_salarios_func (tenant_id, funcionario_id),
                INDEX ix_historico_salarios_vigencia (tenant_id, funcionario_id, vigencia_inicio, vigencia_fim),
                CONSTRAINT fk_historico_salarios_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_historico_salarios_func FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS historico_salarios;");
}
