using System.Data;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class ApiKeyRepository : IApiKeyRepository
{
    private readonly IDataConfiguration _db;
    private const string Cols = "id, tenant_id, nome, key_hash, permissions_json, created_at, created_by, expires_at, revoked_at, last_used_at";

    public ApiKeyRepository(IDataConfiguration db) { _db = db; }

    public Task<ApiKey?> GetByHashAsync(string keyHash, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM api_keys WHERE key_hash = @hash AND revoked_at IS NULL LIMIT 1",
            Map, new Dictionary<string, object?> { ["@hash"] = keyHash }, cancellationToken);

    public Task<IReadOnlyList<ApiKey>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            $"SELECT {Cols} FROM api_keys WHERE tenant_id = @tenant ORDER BY created_at DESC",
            Map, new Dictionary<string, object?> { ["@tenant"] = tenantId }, cancellationToken);

    public Task AddAsync(ApiKey k, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"INSERT INTO api_keys (id, tenant_id, nome, key_hash, permissions_json, created_at, created_by, expires_at)
              VALUES (@id, @tenant_id, @nome, @key_hash, @permissions_json, @created_at, @created_by, @expires_at)",
            new Dictionary<string, object?>
            {
                ["@id"] = k.Id,
                ["@tenant_id"] = k.TenantId,
                ["@nome"] = k.Nome,
                ["@key_hash"] = k.KeyHash,
                ["@permissions_json"] = k.PermissionsJson,
                ["@created_at"] = k.CreatedAt,
                ["@created_by"] = k.CreatedBy,
                ["@expires_at"] = k.ExpiresAt
            }, cancellationToken);

    public Task RevokeAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "UPDATE api_keys SET revoked_at = @now WHERE id = @id",
            new Dictionary<string, object?> { ["@id"] = id, ["@now"] = DateTime.UtcNow },
            cancellationToken);

    public Task TouchLastUsedAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "UPDATE api_keys SET last_used_at = @now WHERE id = @id",
            new Dictionary<string, object?> { ["@id"] = id, ["@now"] = DateTime.UtcNow },
            cancellationToken);

    private static ApiKey Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        KeyHash = r.GetValueOrDefault<string>("key_hash") ?? string.Empty,
        PermissionsJson = r.GetValueOrDefault<string>("permissions_json"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        ExpiresAt = r.GetValueOrDefault<DateTime?>("expires_at"),
        RevokedAt = r.GetValueOrDefault<DateTime?>("revoked_at"),
        LastUsedAt = r.GetValueOrDefault<DateTime?>("last_used_at")
    };
}
