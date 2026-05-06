using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ITokenBlacklistRepository
{
    Task<bool> IsBlacklistedAsync(Guid jti, CancellationToken cancellationToken = default);
    Task AddAsync(TokenBlacklist entry, CancellationToken cancellationToken = default);
    Task<int> PurgeExpiredAsync(CancellationToken cancellationToken = default);
}
