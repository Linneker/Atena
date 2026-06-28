using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations;

/// <summary>
/// Adiciona colunas <c>prova_biometria_local</c> e <c>timestamp_local</c> em
/// <c>marcacoes_ponto</c> para suportar batidas vindas do app MAUI mobile (W3).
/// </summary>
public sealed class V20260630002_AddColunasMobileMarcacoesPonto : IMigration
{
    public long Version => 20260630002;
    public string Name => "AddColunasMobileMarcacoesPonto";

    public void Up(IDbConnection connection, IDbTransaction transaction)
    {
        if (!MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "prova_biometria_local"))
        {
            MigrationHelper.Execute(connection, transaction, @"
                ALTER TABLE marcacoes_ponto
                ADD COLUMN prova_biometria_local TEXT NULL AFTER foto_url;");
        }

        if (!MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "timestamp_local"))
        {
            MigrationHelper.Execute(connection, transaction, @"
                ALTER TABLE marcacoes_ponto
                ADD COLUMN timestamp_local DATETIME(0) NULL AFTER prova_biometria_local;");
        }
    }

    public void Down(IDbConnection connection, IDbTransaction transaction)
    {
        if (MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "timestamp_local"))
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE marcacoes_ponto DROP COLUMN timestamp_local;");
        if (MigrationHelper.ColumnExists(connection, transaction, "marcacoes_ponto", "prova_biometria_local"))
            MigrationHelper.Execute(connection, transaction,
                "ALTER TABLE marcacoes_ponto DROP COLUMN prova_biometria_local;");
    }
}
