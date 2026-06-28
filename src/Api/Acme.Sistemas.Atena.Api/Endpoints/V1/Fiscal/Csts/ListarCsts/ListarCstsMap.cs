using Acme.Sistemas.Services.V1.Cst.Query.ListarCsts;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.Csts.ListarCsts;

public static class ListarCstsMap
{
    public static ListarCstsQuery ToQuery(this ListarCstsRequest r) => new(r.Tipo);

    public static ListarCstsResponse ToResponse(this ListarCstsQueryResult result)
        => new(result.Tipo, result.Items
            .Select(i => new ListarCstsResponseItem(i.Codigo, i.Descricao))
            .ToArray());
}
