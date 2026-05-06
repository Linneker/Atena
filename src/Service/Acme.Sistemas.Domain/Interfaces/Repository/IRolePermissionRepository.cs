using Acme.Sistemas.Domain.Entities.Permissions;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IRolePermissionRepository
{
    Task<IReadOnlyList<string>> GetCodigosByRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetCodigosByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task GrantAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
    Task RevokeAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);
    Task GrantAllToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid? grantedBy, CancellationToken cancellationToken = default);
}
