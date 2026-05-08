using Acme.Sistemas.Services.V1.Roles.Query.ListarPermissoes;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Permissoes.ListarPermissoes;

public static class ListarPermissoesMap
{
    public static ListarPermissoesQuery ToQuery(this ListarPermissoesRequest _) => new();

    public static ListarPermissoesResponse ToResponse(this ListarPermissoesQueryResult result)
        => new(result.Items.Select(i => new ListarPermissoesResponseItem(i.Codigo, i.Recurso, i.Acao, i.Descricao)).ToArray());
}
