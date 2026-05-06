using System.Reflection;
using Acme.Sistemas.Repository.Configuration;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;

public sealed class MigrationRunner
{
    private readonly IDataConfiguration _db;
    private readonly ILogger<MigrationRunner> _logger;
    private const string MigrationTable = "__migrations";

    public MigrationRunner(IDataConfiguration db, ILogger<MigrationRunner> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task RunAsync(Assembly assembly, CancellationToken cancellationToken = default)
    {
        await EnsureMigrationsTableAsync(cancellationToken);

        var applied = await GetAppliedVersionsAsync(cancellationToken);

        var migrations = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IMigration).IsAssignableFrom(t))
            .Select(t => (IMigration)Activator.CreateInstance(t)!)
            .OrderBy(m => m.Version)
            .ToList();

        foreach (var migration in migrations.Where(m => !applied.Contains(m.Version)))
        {
            _logger.LogInformation("Aplicando migration {Version} - {Name}", migration.Version, migration.Name);

            using var conn = _db.CreateConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                migration.Up(conn, tx);

                using var insertCmd = conn.CreateCommand();
                insertCmd.Transaction = tx;
                insertCmd.CommandText = $"INSERT INTO {MigrationTable} (version, name, applied_at) VALUES (@v, @n, @t)";
                AddParameter(insertCmd, "@v", migration.Version);
                AddParameter(insertCmd, "@n", migration.Name);
                AddParameter(insertCmd, "@t", DateTime.UtcNow);
                insertCmd.ExecuteNonQuery();

                tx.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao aplicar migration {Version}", migration.Version);
                tx.Rollback();
                throw;
            }
        }
    }

    private async Task EnsureMigrationsTableAsync(CancellationToken cancellationToken)
    {
        var sql = $@"CREATE TABLE IF NOT EXISTS {MigrationTable} (
            version BIGINT PRIMARY KEY,
            name VARCHAR(255) NOT NULL,
            applied_at DATETIME NOT NULL
        )";
        await _db.ExecuteAsync(sql, null, cancellationToken);
    }

    private async Task<HashSet<long>> GetAppliedVersionsAsync(CancellationToken cancellationToken)
    {
        var versions = await _db.QueryAsync(
            $"SELECT version FROM {MigrationTable}",
            r => r.GetInt64(0),
            null,
            cancellationToken);
        return versions.ToHashSet();
    }

    private static void AddParameter(System.Data.IDbCommand cmd, string name, object? value)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = name;
        p.Value = value ?? DBNull.Value;
        cmd.Parameters.Add(p);
    }
}
