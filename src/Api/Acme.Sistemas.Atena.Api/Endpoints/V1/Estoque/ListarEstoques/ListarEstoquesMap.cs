using Acme.Sistemas.Services.V1.Estoque.Query.ListarEstoques;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ListarEstoques;

public static class ListarEstoquesMap
{
    public static ListarEstoquesQuery ToQuery(this ListarEstoquesRequest request)
        => new(request.Skip, request.Take);

    public static ListarEstoquesResponse ToResponse(this ListarEstoquesQueryResult result)
        => new(
            result.Items.Select(i => new ListarEstoquesResponseItem(
                i.Id, i.Codigo, i.Nome, i.Localizacao, i.Ativo)).ToArray(),
            result.Total);
}
