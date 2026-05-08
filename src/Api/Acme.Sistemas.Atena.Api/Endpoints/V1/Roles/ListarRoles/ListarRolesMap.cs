using Acme.Sistemas.Services.V1.Roles.Query.ListarRoles;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.ListarRoles;

public static class ListarRolesMap
{
    public static ListarRolesQuery ToQuery(this ListarRolesRequest request)
        => new(request.Skip ?? 0, request.Take ?? 50);

    public static ListarRolesResponse ToResponse(this ListarRolesQueryResult result)
        => new(result.Items.Select(i => new ListarRolesResponseItem(i.Id, i.Nome, i.Descricao, i.IsSystem)).ToArray(),
            result.Total);
}
