using System.Data;
using Acme.Sistemas.Domain.Entities;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;

namespace Acme.Sistemas.Repository.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IDataConfiguration Db;
    protected readonly ITenantContext TenantContext;

    protected BaseRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        Db = db;
        TenantContext = tenantContext;
    }

    protected abstract string TableName { get; }
    protected abstract Func<IDataRecord, TEntity> Map { get; }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {TableName} WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL LIMIT 1";
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = id,
            ["@tenantId"] = TenantContext.TenantId
        };
        return await Db.QueryFirstOrDefaultAsync(sql, Map, parameters, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<TEntity>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default)
    {
        var sql = $@"SELECT * FROM {TableName}
                     WHERE tenant_id = @tenantId AND deleted_at IS NULL
                     ORDER BY created_at DESC
                     LIMIT @take OFFSET @skip";
        var parameters = new Dictionary<string, object?>
        {
            ["@tenantId"] = TenantContext.TenantId,
            ["@skip"] = skip,
            ["@take"] = take
        };
        return await Db.QueryAsync(sql, Map, parameters, cancellationToken);
    }

    public abstract Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    public abstract Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sql = $@"UPDATE {TableName}
                     SET deleted_at = @deletedAt, deleted_by = @deletedBy
                     WHERE id = @id AND tenant_id = @tenantId";
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = id,
            ["@tenantId"] = TenantContext.TenantId,
            ["@deletedAt"] = DateTime.UtcNow,
            ["@deletedBy"] = TenantContext.UserId
        };
        await Db.ExecuteAsync(sql, parameters, cancellationToken);
    }

    public virtual async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT COUNT(*) FROM {TableName} WHERE tenant_id = @tenantId AND deleted_at IS NULL";
        var parameters = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        return await Db.ExecuteScalarAsync<long>(sql, parameters, cancellationToken);
    }
}
