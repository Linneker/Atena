using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260628009_AddTabelaEscalasFuncionario : IMigration
{
    public long Version => 20260628009;
    public string Name => "AddTabelaEscalasFuncionario";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS escalas_funcionario (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                jornada_id CHAR(36) NOT NULL,
                vigencia_inicio DATE NOT NULL,
                vigencia_fim DATE NULL,
                observacao TEXT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_escalas_func_tenant (tenant_id),
                INDEX ix_escalas_func_func (tenant_id, funcionario_id),
                INDEX ix_escalas_func_jornada (tenant_id, jornada_id),
                INDEX ix_escalas_func_vigencia (tenant_id, funcionario_id, vigencia_inicio, vigencia_fim),
                CONSTRAINT fk_escalas_func_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_escalas_func_func FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id) ON DELETE CASCADE,
                CONSTRAINT fk_escalas_func_jornada FOREIGN KEY (jornada_id) REFERENCES jornadas(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS escalas_funcionario;");
}
