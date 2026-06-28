using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Marca as colunas texto legadas <c>funcionarios.cargo</c> e <c>funcionarios.departamento</c>
/// como obsoletas via COMMENT do MySQL. As colunas continuam existindo por 2 ondas
/// (até W3) para suportar rollback. Nada de dados é alterado nesta migration.
/// </summary>
public sealed class V20260628014_MarcarCamposObsoletosEmFuncionarios : IMigration
{
    public long Version => 20260628014;
    public string Name => "MarcarCamposObsoletosEmFuncionarios";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            ALTER TABLE funcionarios
              MODIFY COLUMN cargo VARCHAR(100) NULL COMMENT 'OBSOLETO desde 2026-06-28 (rh-fundacao). Use cargo_id. Remoção planejada para W3.';");

        MigrationHelper.Execute(connection, transaction, @"
            ALTER TABLE funcionarios
              MODIFY COLUMN departamento VARCHAR(100) NULL COMMENT 'OBSOLETO desde 2026-06-28 (rh-fundacao). Use departamento_id. Remoção planejada para W3.';");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        MigrationHelper.Execute(connection, transaction, @"
            ALTER TABLE funcionarios MODIFY COLUMN cargo VARCHAR(100) NULL;");
        MigrationHelper.Execute(connection, transaction, @"
            ALTER TABLE funcionarios MODIFY COLUMN departamento VARCHAR(100) NULL;");
    }
}
