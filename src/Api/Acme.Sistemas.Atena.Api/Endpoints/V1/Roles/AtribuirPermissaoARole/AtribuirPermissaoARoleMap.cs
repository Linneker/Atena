using Acme.Sistemas.Services.V1.Roles.Command.AtribuirPermissaoARole;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.AtribuirPermissaoARole;

public static class AtribuirPermissaoARoleMap
{
    public static AtribuirPermissaoARoleCommand ToCommand(this AtribuirPermissaoARoleRequest request, Guid roleId)
        => new(roleId, request.PermissaoCodigo);
}
