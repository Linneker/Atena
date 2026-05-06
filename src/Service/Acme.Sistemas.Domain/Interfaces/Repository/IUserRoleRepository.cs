using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IUserRoleRepository
{
    Task<IReadOnlyList<UserRole>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AssignAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task RevokeAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
}
