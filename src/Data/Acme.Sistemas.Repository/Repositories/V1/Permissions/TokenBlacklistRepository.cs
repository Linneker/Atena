using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class TokenBlacklistRepository : ITokenBlacklistRepository
{
    private readonly IDataConfiguration _db;

    public TokenBlacklistRepository(IDataConfiguration db) { _db = db; }

    public async Task<bool> IsBlacklistedAsync(Guid jti, CancellationToken cancellationToken = default)
    {
        var count = await _db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM token_blacklist WHERE jti = @jti AND expires_at > UTC_TIMESTAMP()",
            new Dictionary<string, object?> { ["@jti"] = jti },
            cancellationToken);
        return count > 0;
    }

    public Task AddAsync(TokenBlacklist e, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"INSERT IGNORE INTO token_blacklist (jti, tenant_id, user_id, blacklisted_at, expires_at, reason)
              VALUES (@jti, @tenant_id, @user_id, @blacklisted_at, @expires_at, @reason)",
            new Dictionary<string, object?>
            {
                ["@jti"] = e.Jti,
                ["@tenant_id"] = e.TenantId,
                ["@user_id"] = e.UserId,
                ["@blacklisted_at"] = e.BlacklistedAt,
                ["@expires_at"] = e.ExpiresAt,
                ["@reason"] = e.Reason
            }, cancellationToken);

    public Task<int> PurgeExpiredAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "DELETE FROM token_blacklist WHERE expires_at <= UTC_TIMESTAMP()",
            null, cancellationToken);
}
