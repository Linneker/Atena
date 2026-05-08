using Acme.Sistemas.Services.V1.Divida.Query.ListarDividas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ListarDividas;

public static class ListarDividasMap
{
    public static ListarDividasQuery ToQuery(this ListarDividasRequest request)
        => new(request.Status, request.Skip ?? 0, request.Take ?? 50);

    public static ListarDividasResponse ToResponse(this ListarDividasQueryResult result)
        => new(result.Items.Select(i => new ListarDividasResponseItem(
            i.Id, i.Credor, i.ValorOriginal, i.ValorPago, i.Saldo,
            i.DataInicio, i.DataFim, i.NumeroParcelas, i.Status)).ToArray(),
            result.Total);
}
