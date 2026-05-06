using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirPermissaoARole;

public sealed class AtribuirPermissaoARoleCommandHandler
    : IRequestHandler<AtribuirPermissaoARoleCommand, ResponseDefault>
{
    private readonly IRoleRepository _roles;
    private readonly IPermissionRepository _permissions;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly ITenantContext _tenantContext;

    public AtribuirPermissaoARoleCommandHandler(
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

    public async Task<ResponseDefault> Handle(
        AtribuirPermissaoARoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null) return ResponseDefault.BadRequest(Error.NotFound("Role não encontrada."));

        var perm = await _permissions.GetByCodigoAsync(request.PermissaoCodigo, cancellationToken);
        if (perm is null) return ResponseDefault.BadRequest(Error.NotFound("Permissão não encontrada."));

        await _rolePermissions.GrantAsync(new RolePermission
        {
            RoleId = role.Id,
            PermissionId = perm.Id,
            GrantedAt = DateTime.UtcNow,
            GrantedBy = _tenantContext.UserId
        }, cancellationToken);

        return ResponseDefault.NoContent();
    }
}
