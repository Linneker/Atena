using System.Data;
using Acme.Sistemas.Infrastructure.Databases.Configuration;

namespace Acme.Sistemas.Infrastructure.Databases.Configuration;

public sealed class TransactionManager
{
    private readonly IDataConfiguration _db;

    public TransactionManager(IDataConfiguration db)
    {
        _db = db;
    }

    public async Task<T> ExecuteAsync<T>(Func<IDbTransaction, Task<T>> work, CancellationToken cancellationToken = default)
    {
        using var conn = _db.CreateConnection();
        conn.Open();
        using var tx = conn.BeginTransaction();
        try
        {
            var result = await work(tx);
            tx.Commit();
            return result;
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
}
