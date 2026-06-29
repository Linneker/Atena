using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Flag <c>usa_rep_oficial</c> em <c>empresas</c>. Quando true, batidas exigem NSR +
/// geram <c>ComprovantePonto</c> assinado ICP-Brasil. Default false (compat W2).
/// </summary>
public sealed class V20260629007_AlterarEmpresasAdicionarUsaRepOficial : IMigration
{
    public long Version => 20260629007;
    public string Name => "AlterarEmpresasAdicionarUsaRepOficial";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        if (!MigrationHelper.ColumnExists(connection, transaction, "empresas", "usa_rep_oficial"))
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE empresas ADD COLUMN usa_rep_oficial TINYINT(1) NOT NULL DEFAULT 0;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        if (MigrationHelper.ColumnExists(connection, transaction, "empresas", "usa_rep_oficial"))
            MigrationHelper.Execute(connection, transaction, "ALTER TABLE empresas DROP COLUMN usa_rep_oficial;");
    }
}
