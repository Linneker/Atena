using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IApiKeyRepository
{
    Task<ApiKey?> GetByHashAsync(string keyHash, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApiKey>> ListByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(ApiKey apiKey, CancellationToken cancellationToken = default);
    Task RevokeAsync(Guid id, CancellationToken cancellationToken = default);
    Task TouchLastUsedAsync(Guid id, CancellationToken cancellationToken = default);
}
