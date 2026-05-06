using System.Data;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

public static class MigrationHelper
{
    public static void Execute(IDbConnection connection, IDbTransaction transaction, string sql)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = sql;
        cmd.CommandType = CommandType.Text;
        cmd.ExecuteNonQuery();
    }

    public static bool TableExists(IDbConnection connection, IDbTransaction transaction, string tableName)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = @t";
        var p = cmd.CreateParameter();
        p.ParameterName = "@t";
        p.Value = tableName;
        cmd.Parameters.Add(p);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    public static bool ColumnExists(IDbConnection connection, IDbTransaction transaction, string tableName, string columnName)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = @"SELECT COUNT(*) FROM information_schema.columns
                            WHERE table_schema = DATABASE() AND table_name = @t AND column_name = @c";
        var t = cmd.CreateParameter(); t.ParameterName = "@t"; t.Value = tableName; cmd.Parameters.Add(t);
        var c = cmd.CreateParameter(); c.ParameterName = "@c"; c.Value = columnName; cmd.Parameters.Add(c);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }
}
