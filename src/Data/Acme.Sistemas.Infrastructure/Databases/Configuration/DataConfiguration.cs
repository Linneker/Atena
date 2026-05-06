using System.Data;
using Acme.Sistemas.Repository.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Acme.Sistemas.Infrastructure.Databases.Configuration;

public sealed class DataConfiguration : IDataConfiguration
{
    private readonly string _connectionString;
    private readonly ILogger<DataConfiguration> _logger;
    private readonly RetryPolicy _retryPolicy;

    public DataConfiguration(IConfiguration configuration, ILogger<DataConfiguration> logger, RetryPolicy retryPolicy)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default não configurada.");
        _logger = logger;
        _retryPolicy = retryPolicy;
    }

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public async Task<IReadOnlyList<T>> QueryAsync<T>(
        string sql,
        Func<IDataRecord, T> map,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);
            await using var cmd = BuildCommand(conn, sql, parameters);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            var list = new List<T>();
            while (await reader.ReadAsync(cancellationToken))
            {
                list.Add(map(reader));
            }
            return (IReadOnlyList<T>)list;
        }, _logger, cancellationToken);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        Func<IDataRecord, T> map,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);
            await using var cmd = BuildCommand(conn, sql, parameters);
            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            return await reader.ReadAsync(cancellationToken) ? map(reader) : default;
        }, _logger, cancellationToken);
    }

    public async Task<int> ExecuteAsync(
        string sql,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);
            await using var cmd = BuildCommand(conn, sql, parameters);
            return await cmd.ExecuteNonQueryAsync(cancellationToken);
        }, _logger, cancellationToken);
    }

    public async Task<TScalar?> ExecuteScalarAsync<TScalar>(
        string sql,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            await using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);
            await using var cmd = BuildCommand(conn, sql, parameters);
            var raw = await cmd.ExecuteScalarAsync(cancellationToken);
            if (raw is null || raw is DBNull) return default;
            return (TScalar)Convert.ChangeType(raw, typeof(TScalar));
        }, _logger, cancellationToken);
    }

    private static MySqlCommand BuildCommand(MySqlConnection conn, string sql, IDictionary<string, object?>? parameters)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.CommandType = CommandType.Text;
        if (parameters is not null)
        {
            foreach (var kvp in parameters)
            {
                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value ?? DBNull.Value);
            }
        }
        return cmd;
    }
}
