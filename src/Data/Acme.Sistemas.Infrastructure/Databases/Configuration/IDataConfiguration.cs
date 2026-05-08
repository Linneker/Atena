using System.Data;

namespace Acme.Sistemas.Infrastructure.Databases.Configuration;

public interface IDataConfiguration
{
    IDbConnection CreateConnection();

    Task<IReadOnlyList<T>> QueryAsync<T>(
        string sql,
        Func<IDataRecord, T> map,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default);

    Task<T?> QueryFirstOrDefaultAsync<T>(
        string sql,
        Func<IDataRecord, T> map,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(
        string sql,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default);

    Task<TScalar?> ExecuteScalarAsync<TScalar>(
        string sql,
        IDictionary<string, object?>? parameters = null,
        CancellationToken cancellationToken = default);
}
