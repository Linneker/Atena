using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IPermissionRepository
{
    Task<Permission?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Permission>> ListAllAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(Permission permission, CancellationToken cancellationToken = default);
}
