using Acme.Sistemas.Services.V1.Uf.Query.ListarUfs;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Cadastros.Ufs.ListarUfs;

public static class ListarUfsMap
{
    public static ListarUfsQuery ToQuery(this ListarUfsRequest _) => new();

    public static ListarUfsResponse ToResponse(this ListarUfsQueryResult result)
        => new(result.Items
            .Select(i => new ListarUfsResponseItem(i.Sigla, i.Nome, i.CodigoIbge))
            .ToArray());
}
