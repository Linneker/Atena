using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Roles.Command.CriarRole;

public sealed class CriarRoleCommandHandler
    : IRequestHandler<CriarRoleCommand, ResponseDefault<CriarRoleCommandResult>>
{
    private readonly IRoleRepository _roles;
    private readonly IPermissionRepository _permissions;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly ITenantContext _tenantContext;

    public CriarRoleCommandHandler(
        IRoleRepository roles,
        IPermissionRepository permissions,
        IRolePermissionRepository rolePermissions,
        ITenantContext tenantContext)
    {
        _roles = roles;
        _permissions = permissions;
        _rolePermissions = rolePermissions;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarRoleCommandResult>> Handle(
        CriarRoleCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _roles.GetByNomeAsync(request.Nome, cancellationToken);
        if (existing is not null)
        {
            return ResponseDefault<CriarRoleCommandResult>.Conflict(
                $"Já existe role com o nome '{request.Nome}' neste tenant.");
        }

        var role = new Role
        {
            TenantId = _tenantContext.TenantId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            CreatedBy = _tenantContext.UserId
        };

        await _roles.AddAsync(role, cancellationToken);

        if (request.PermissoesCodigos is { Count: > 0 })
        {
            var permissionIds = new List<Guid>();
            foreach (var codigo in request.PermissoesCodigos)
            {
                var perm = await _permissions.GetByCodigoAsync(codigo, cancellationToken);
                if (perm is not null) permissionIds.Add(perm.Id);
            }
            await _rolePermissions.GrantAllToRoleAsync(role.Id, permissionIds, _tenantContext.UserId, cancellationToken);
        }

        return ResponseDefault<CriarRoleCommandResult>.Created(
            new CriarRoleCommandResult(role.Id, role.Nome));
    }
}
