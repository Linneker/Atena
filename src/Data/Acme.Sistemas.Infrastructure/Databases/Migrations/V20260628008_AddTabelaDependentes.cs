using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260628008_AddTabelaDependentes : IMigration
{
    public long Version => 20260628008;
    public string Name => "AddTabelaDependentes";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS dependentes (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                nome_completo VARCHAR(200) NOT NULL,
                cpf CHAR(11) NULL,
                data_nascimento DATE NOT NULL,
                tipo VARCHAR(20) NOT NULL,
                irrf TINYINT(1) NOT NULL DEFAULT 0,
                salario_familia TINYINT(1) NOT NULL DEFAULT 0,
                pensao_alimenticia_pct DECIMAL(5,2) NULL,
                data_inicio DATE NULL,
                data_fim DATE NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_dependentes_tenant (tenant_id),
                INDEX ix_dependentes_func (tenant_id, funcionario_id),
                CONSTRAINT fk_dependentes_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_dependentes_func FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id) ON DELETE CASCADE
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS dependentes;");
}
