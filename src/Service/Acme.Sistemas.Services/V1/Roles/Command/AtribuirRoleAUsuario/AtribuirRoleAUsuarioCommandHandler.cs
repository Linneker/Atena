using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirRoleAUsuario;

public sealed class AtribuirRoleAUsuarioCommandHandler
    : IRequestHandler<AtribuirRoleAUsuarioCommand, ResponseDefault>
{
    private readonly IRoleRepository _roles;
    private readonly IUsuarioRepository _users;
    private readonly IUserRoleRepository _userRoles;
    private readonly ITenantContext _tenantContext;

    public AtribuirRoleAUsuarioCommandHandler(
        IRoleRepository roles,
        IUsuarioRepository users,
        IUserRoleRepository userRoles,
        ITenantContext tenantContext)
    {
        _roles = roles;
        _users = users;
        _userRoles = userRoles;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault> Handle(
        AtribuirRoleAUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(request.RoleId, cancellationToken);
        if (role is null) return ResponseDefault.BadRequest(Error.NotFound("Role não encontrada."));

        var user = await _users.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null) return ResponseDefault.BadRequest(Error.NotFound("Usuário não encontrado."));

        await _userRoles.AssignAsync(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            TenantId = _tenantContext.TenantId,
            GrantedAt = DateTime.UtcNow,
            GrantedBy = _tenantContext.UserId,
            ExpiresAt = request.ExpiresAt
        }, cancellationToken);

        return ResponseDefault.NoContent();
    }
}
