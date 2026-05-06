using Acme.Sistemas.Domain.Entities.Tenants;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tenant?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tenant>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default);
    Task<TenantLimites?> GetLimitesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task UpsertLimitesAsync(TenantLimites limites, CancellationToken cancellationToken = default);
}
