using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Role?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Role>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task AddAsync(Role role, CancellationToken cancellationToken = default);
    Task UpdateAsync(Role role, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default);
}
