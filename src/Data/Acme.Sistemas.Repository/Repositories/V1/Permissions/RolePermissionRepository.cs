using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class RolePermissionRepository : IRolePermissionRepository
{
    private readonly IDataConfiguration _db;

    public RolePermissionRepository(IDataConfiguration db) { _db = db; }

    public Task<IReadOnlyList<string>> GetCodigosByRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            @"SELECT p.codigo FROM permissions p
              INNER JOIN role_permissions rp ON rp.permission_id = p.id
              WHERE rp.role_id = @roleId",
            r => r.GetString(0),
            new Dictionary<string, object?> { ["@roleId"] = roleId },
            cancellationToken);

    public Task<IReadOnlyList<string>> GetCodigosByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            @"SELECT DISTINCT p.codigo FROM permissions p
              INNER JOIN role_permissions rp ON rp.permission_id = p.id
              INNER JOIN user_roles ur ON ur.role_id = rp.role_id
              INNER JOIN roles r ON r.id = ur.role_id AND r.deleted_at IS NULL
              WHERE ur.user_id = @userId
                AND (ur.expires_at IS NULL OR ur.expires_at > UTC_TIMESTAMP())",
            r => r.GetString(0),
            new Dictionary<string, object?> { ["@userId"] = userId },
            cancellationToken);

    public Task GrantAsync(RolePermission rp, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"INSERT IGNORE INTO role_permissions (role_id, permission_id, granted_at, granted_by)
              VALUES (@role_id, @permission_id, @granted_at, @granted_by)",
            new Dictionary<string, object?>
            {
                ["@role_id"] = rp.RoleId,
                ["@permission_id"] = rp.PermissionId,
                ["@granted_at"] = rp.GrantedAt,
                ["@granted_by"] = rp.GrantedBy
            },
            cancellationToken);

    public Task RevokeAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "DELETE FROM role_permissions WHERE role_id = @role_id AND permission_id = @permission_id",
            new Dictionary<string, object?> { ["@role_id"] = roleId, ["@permission_id"] = permissionId },
            cancellationToken);

    public async Task GrantAllToRoleAsync(Guid roleId, IEnumerable<Guid> permissionIds, Guid? grantedBy, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var pid in permissionIds)
        {
            await GrantAsync(new RolePermission
            {
                RoleId = roleId,
                PermissionId = pid,
                GrantedAt = now,
                GrantedBy = grantedBy
            }, cancellationToken);
        }
    }
}
