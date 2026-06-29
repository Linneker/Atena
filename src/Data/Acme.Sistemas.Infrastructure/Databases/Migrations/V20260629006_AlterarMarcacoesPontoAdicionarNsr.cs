using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Adiciona <c>nsr</c> e <c>comprovante_id</c> em <c>marcacoes_ponto</c> para batidas
/// emitidas via REP oficial (Portaria 671). Ambos NULL em empresas com <c>usa_rep_oficial=false</c>.
/// </summary>
public sealed class V20260629006_AlterarMarcacoesPontoAdicionarNsr : IMigration
{
    public long Version => 20260629006;
    public string Name => "AlterarMarcacoesPontoAdicionarNsr";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        if (!MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "nsr"))
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE marcacoes_ponto ADD COLUMN nsr BIGINT NULL AFTER hash_integridade;");

        if (!MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "comprovante_id"))
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE marcacoes_ponto ADD COLUMN comprovante_id CHAR(36) NULL AFTER nsr;");
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        if (MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "comprovante_id"))
            MigrationHelper.Execute(connection, transaction, "ALTER TABLE marcacoes_ponto DROP COLUMN comprovante_id;");
        if (MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "nsr"))
            MigrationHelper.Execute(connection, transaction, "ALTER TABLE marcacoes_ponto DROP COLUMN nsr;");
    }
}
