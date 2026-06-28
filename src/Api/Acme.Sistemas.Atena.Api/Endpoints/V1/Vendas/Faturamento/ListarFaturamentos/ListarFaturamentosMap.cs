using Acme.Sistemas.Services.V1.Faturamento.Query.ListarFaturamentos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.ListarFaturamentos;

public static class ListarFaturamentosMap
{
    public static ListarFaturamentosQuery ToQuery(this ListarFaturamentosRequest request)
        => new(request.Inicio, request.Fim, request.Skip, request.Take);

    public static ListarFaturamentosResponse ToResponse(this ListarFaturamentosQueryResult result)
        => new(
            result.Items.Select(i => i.ToResponseItem()).ToArray(),
            result.Total, result.Skip, result.Take);

    private static ListarFaturamentosResponseItem ToResponseItem(this ListarFaturamentosQueryItem item)
        => new(item.Id, item.Numero, item.PedidoVendaId,
            item.DataFaturamento, item.Tipo, item.ValorTotal,
            item.NFeId, item.ContaReceberId);
}
