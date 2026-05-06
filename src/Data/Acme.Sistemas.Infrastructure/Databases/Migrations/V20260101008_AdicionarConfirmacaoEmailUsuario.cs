using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

public sealed class V20260101008_AdicionarConfirmacaoEmailUsuario : IMigration
{
    public long Version => 20260101008;
    public string Name => "AdicionarConfirmacaoEmailUsuario";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        if (!MigrationHelper.ColumnExists(connection, transaction, "usuarios", "email_confirmed_at"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE usuarios ADD COLUMN email_confirmed_at DATETIME NULL;");
        }
        if (!MigrationHelper.ColumnExists(connection, transaction, "usuarios", "email_confirmation_token_hash"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE usuarios ADD COLUMN email_confirmation_token_hash VARCHAR(256) NULL;");
            MigrationHelper.Execute(connection, transaction,
                "CREATE INDEX ix_usuarios_email_confirm_token ON usuarios (email_confirmation_token_hash);");
        }
        if (!MigrationHelper.ColumnExists(connection, transaction, "usuarios", "email_confirmation_expires_at"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE usuarios ADD COLUMN email_confirmation_expires_at DATETIME NULL;");
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        if (MigrationHelper.ColumnExists(connection, transaction, "usuarios", "email_confirmation_token_hash"))
        {
            MigrationHelper.Execute(connection, transaction,
                "DROP INDEX ix_usuarios_email_confirm_token ON usuarios;");
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE usuarios DROP COLUMN email_confirmation_token_hash;");
        }
        if (MigrationHelper.ColumnExists(connection, transaction, "usuarios", "email_confirmation_expires_at"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE usuarios DROP COLUMN email_confirmation_expires_at;");
        }
        if (MigrationHelper.ColumnExists(connection, transaction, "usuarios", "email_confirmed_at"))
        {
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE usuarios DROP COLUMN email_confirmed_at;");
        }
    }
}
