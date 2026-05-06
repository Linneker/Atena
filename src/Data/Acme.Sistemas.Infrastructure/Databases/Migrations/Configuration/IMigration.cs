using System.Data;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

public interface IMigration
{
    long Version { get; }
    string Name { get; }
    void Up(IDbConnection connection, IDbTransaction transaction);
    void Down(IDbConnection connection, IDbTransaction transaction);
}
