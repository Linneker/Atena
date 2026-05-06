using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
    Task RevokeAsync(Guid id, Guid? replacedBy, CancellationToken cancellationToken = default);
    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
