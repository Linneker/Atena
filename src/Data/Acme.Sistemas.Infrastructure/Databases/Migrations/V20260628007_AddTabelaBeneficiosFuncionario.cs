using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260628007_AddTabelaBeneficiosFuncionario : IMigration
{
    public long Version => 20260628007;
    public string Name => "AddTabelaBeneficiosFuncionario";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            CREATE TABLE IF NOT EXISTS beneficios_funcionario (
                id CHAR(36) NOT NULL PRIMARY KEY,
                tenant_id CHAR(36) NOT NULL,
                funcionario_id CHAR(36) NOT NULL,
                beneficio_catalogo_id CHAR(36) NOT NULL,
                valor DECIMAL(10,2) NULL,
                desconto_funcionario_pct DECIMAL(5,2) NULL,
                vigencia_inicio DATE NOT NULL,
                vigencia_fim DATE NULL,
                observacao TEXT NULL,
                created_at DATETIME NOT NULL,
                created_by CHAR(36) NULL,
                updated_at DATETIME NULL,
                updated_by CHAR(36) NULL,
                deleted_at DATETIME NULL,
                deleted_by CHAR(36) NULL,
                INDEX ix_beneficios_func_tenant (tenant_id),
                INDEX ix_beneficios_func_func (tenant_id, funcionario_id),
                INDEX ix_beneficios_func_cat (tenant_id, beneficio_catalogo_id),
                CONSTRAINT fk_beneficios_func_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id) ON DELETE CASCADE,
                CONSTRAINT fk_beneficios_func_func FOREIGN KEY (funcionario_id) REFERENCES funcionarios(id) ON DELETE CASCADE,
                CONSTRAINT fk_beneficios_func_cat FOREIGN KEY (beneficio_catalogo_id) REFERENCES beneficios_catalogo(id)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
        => MigrationHelper.Execute(connection, transaction, "DROP TABLE IF EXISTS beneficios_funcionario;");
}
