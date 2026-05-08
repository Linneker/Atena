using Acme.Sistemas.Services.V1.Roles.Command.AtribuirRoleAUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.AtribuirRoleAUsuario;

public static class AtribuirRoleAUsuarioMap
{
    public static AtribuirRoleAUsuarioCommand ToCommand(this AtribuirRoleAUsuarioRequest request, Guid roleId)
        => new(request.UserId, roleId, request.ExpiresAt);
}
