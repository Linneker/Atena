using System.Data;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class RoleRepository : IRoleRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    private const string Cols = "id, tenant_id, nome, descricao, is_system, created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public RoleRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM roles WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@id"] = id, ["@tenantId"] = _tenantContext.TenantId },
            cancellationToken);

    public Task<Role?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM roles WHERE tenant_id = @tenantId AND nome = @nome AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId, ["@nome"] = nome },
            cancellationToken);

    public Task<IReadOnlyList<Role>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            $"SELECT {Cols} FROM roles WHERE tenant_id = @tenantId AND deleted_at IS NULL ORDER BY nome LIMIT @take OFFSET @skip",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId, ["@skip"] = skip, ["@take"] = take },
            cancellationToken);

    public Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO roles
            (id, tenant_id, nome, descricao, is_system, created_at, created_by)
            VALUES (@id, @tenant_id, @nome, @descricao, @is_system, @created_at, @created_by)";
        return _db.ExecuteAsync(sql, new Dictionary<string, object?>
        {
            ["@id"] = role.Id,
            ["@tenant_id"] = role.TenantId,
            ["@nome"] = role.Nome,
            ["@descricao"] = role.Descricao,
            ["@is_system"] = role.IsSystem ? 1 : 0,
            ["@created_at"] = role.CreatedAt,
            ["@created_by"] = role.CreatedBy
        }, cancellationToken);
    }

    public Task UpdateAsync(Role role, CancellationToken cancellationToken = default)
    {
        const string sql = @"UPDATE roles SET nome = @nome, descricao = @descricao,
            updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenant_id";
        return _db.ExecuteAsync(sql, new Dictionary<string, object?>
        {
            ["@id"] = role.Id,
            ["@tenant_id"] = _tenantContext.TenantId,
            ["@nome"] = role.Nome,
            ["@descricao"] = role.Descricao,
            ["@updated_at"] = DateTime.UtcNow,
            ["@updated_by"] = role.UpdatedBy
        }, cancellationToken);
    }

    public Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default)
    {
        const string sql = @"UPDATE roles SET deleted_at = @deleted_at, deleted_by = @deleted_by
            WHERE id = @id AND tenant_id = @tenant_id AND is_system = 0";
        return _db.ExecuteAsync(sql, new Dictionary<string, object?>
        {
            ["@id"] = id,
            ["@tenant_id"] = _tenantContext.TenantId,
            ["@deleted_at"] = DateTime.UtcNow,
            ["@deleted_by"] = deletedBy
        }, cancellationToken);
    }

    private static Role Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        IsSystem = r.GetValueOrDefault<int>("is_system") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
