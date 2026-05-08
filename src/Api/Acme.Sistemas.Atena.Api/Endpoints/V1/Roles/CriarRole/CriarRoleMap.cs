using Acme.Sistemas.Services.V1.Roles.Command.CriarRole;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.CriarRole;

public static class CriarRoleMap
{
    public static CriarRoleCommand ToCommand(this CriarRoleRequest request)
        => new(request.Nome, request.Descricao, request.PermissoesCodigos);

    public static CriarRoleResponse ToResponse(this CriarRoleCommandResult result)
        => new(result.Id, result.Nome);
}
