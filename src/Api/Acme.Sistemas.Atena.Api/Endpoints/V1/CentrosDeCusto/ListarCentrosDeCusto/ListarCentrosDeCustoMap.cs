using Acme.Sistemas.Services.V1.CentroDeCusto.Query.ListarCentrosDeCusto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.ListarCentrosDeCusto;

public static class ListarCentrosDeCustoMap
{
    public static ListarCentrosDeCustoQuery ToQuery(this ListarCentrosDeCustoRequest request)
        => new(request.Skip, request.Take);

    public static ListarCentrosDeCustoResponse ToResponse(this ListarCentrosDeCustoQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray());

    private static ListarCentrosDeCustoResponseItem ToResponseItem(this ListarCentrosDeCustoQueryItem item)
        => new(item.Id, item.Codigo, item.Nome, item.Descricao, item.ResponsavelId, item.Ativo);
}
