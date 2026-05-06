using System.Data;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDataConfiguration _db;
    private const string Cols = "id, tenant_id, user_id, token_hash, jti, issued_at, expires_at, revoked_at, replaced_by, user_agent, ip_address";

    public RefreshTokenRepository(IDataConfiguration db) { _db = db; }

    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM refresh_tokens WHERE token_hash = @hash LIMIT 1",
            Map, new Dictionary<string, object?> { ["@hash"] = tokenHash }, cancellationToken);

    public Task AddAsync(RefreshToken t, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"INSERT INTO refresh_tokens
              (id, tenant_id, user_id, token_hash, jti, issued_at, expires_at, user_agent, ip_address)
              VALUES (@id, @tenant_id, @user_id, @token_hash, @jti, @issued_at, @expires_at, @user_agent, @ip_address)",
            new Dictionary<string, object?>
            {
                ["@id"] = t.Id,
                ["@tenant_id"] = t.TenantId,
                ["@user_id"] = t.UserId,
                ["@token_hash"] = t.TokenHash,
                ["@jti"] = t.Jti,
                ["@issued_at"] = t.IssuedAt,
                ["@expires_at"] = t.ExpiresAt,
                ["@user_agent"] = t.UserAgent,
                ["@ip_address"] = t.IpAddress
            }, cancellationToken);

    public Task RevokeAsync(Guid id, Guid? replacedBy, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "UPDATE refresh_tokens SET revoked_at = @now, replaced_by = @replaced WHERE id = @id",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@now"] = DateTime.UtcNow,
                ["@replaced"] = replacedBy
            }, cancellationToken);

    public Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "UPDATE refresh_tokens SET revoked_at = @now WHERE user_id = @user_id AND revoked_at IS NULL",
            new Dictionary<string, object?> { ["@user_id"] = userId, ["@now"] = DateTime.UtcNow },
            cancellationToken);

    private static RefreshToken Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        UserId = r.GetValueOrDefault<Guid>("user_id"),
        TokenHash = r.GetValueOrDefault<string>("token_hash") ?? string.Empty,
        Jti = r.GetValueOrDefault<Guid>("jti"),
        IssuedAt = r.GetValueOrDefault<DateTime>("issued_at"),
        ExpiresAt = r.GetValueOrDefault<DateTime>("expires_at"),
        RevokedAt = r.GetValueOrDefault<DateTime?>("revoked_at"),
        ReplacedBy = r.GetValueOrDefault<Guid?>("replaced_by"),
        UserAgent = r.GetValueOrDefault<string>("user_agent"),
        IpAddress = r.GetValueOrDefault<string>("ip_address")
    };
}
