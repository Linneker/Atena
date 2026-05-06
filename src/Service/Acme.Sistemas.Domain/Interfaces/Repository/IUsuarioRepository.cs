using Acme.Sistemas.Domain.Entities.Users;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByIdAcrossTenantsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByEmailAcrossTenantsAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Usuario>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default);
    Task UpdateLoginStatusAsync(Guid id, int failedAttempts, DateTime? lockedUntil, DateTime? lastLoginAt, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default);
}
