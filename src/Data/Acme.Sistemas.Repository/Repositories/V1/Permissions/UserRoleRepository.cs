using System.Data;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class UserRoleRepository : IUserRoleRepository
{
    private readonly IDataConfiguration _db;

    public UserRoleRepository(IDataConfiguration db) { _db = db; }

    public Task<IReadOnlyList<UserRole>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            @"SELECT user_id, role_id, tenant_id, granted_at, granted_by, expires_at
              FROM user_roles WHERE user_id = @userId",
            Map,
            new Dictionary<string, object?> { ["@userId"] = userId },
            cancellationToken);

    public Task AssignAsync(UserRole ur, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"INSERT INTO user_roles (user_id, role_id, tenant_id, granted_at, granted_by, expires_at)
              VALUES (@user_id, @role_id, @tenant_id, @granted_at, @granted_by, @expires_at)
              ON DUPLICATE KEY UPDATE granted_at = @granted_at, granted_by = @granted_by, expires_at = @expires_at",
            new Dictionary<string, object?>
            {
                ["@user_id"] = ur.UserId,
                ["@role_id"] = ur.RoleId,
                ["@tenant_id"] = ur.TenantId,
                ["@granted_at"] = ur.GrantedAt,
                ["@granted_by"] = ur.GrantedBy,
                ["@expires_at"] = ur.ExpiresAt
            }, cancellationToken);

    public Task RevokeAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            "DELETE FROM user_roles WHERE user_id = @user_id AND role_id = @role_id",
            new Dictionary<string, object?> { ["@user_id"] = userId, ["@role_id"] = roleId },
            cancellationToken);

    private static UserRole Map(IDataRecord r) => new()
    {
        UserId = r.GetValueOrDefault<Guid>("user_id"),
        RoleId = r.GetValueOrDefault<Guid>("role_id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        GrantedAt = r.GetValueOrDefault<DateTime>("granted_at"),
        GrantedBy = r.GetValueOrDefault<Guid?>("granted_by"),
        ExpiresAt = r.GetValueOrDefault<DateTime?>("expires_at")
    };
}
