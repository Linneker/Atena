using Acme.Sistemas.Services.V1.Cfop.Query.ListarCfops;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.Cfops.ListarCfops;

public static class ListarCfopsMap
{
    public static ListarCfopsQuery ToQuery(this ListarCfopsRequest r) => new(r.Categoria);

    public static ListarCfopsResponse ToResponse(this ListarCfopsQueryResult result)
        => new(result.Items
            .Select(i => new ListarCfopsResponseItem(i.Codigo, i.Descricao, i.Categoria))
            .ToArray());
}
