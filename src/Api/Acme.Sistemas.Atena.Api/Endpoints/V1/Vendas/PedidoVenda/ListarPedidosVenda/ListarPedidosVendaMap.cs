using Acme.Sistemas.Services.V1.PedidoVenda.Query.ListarPedidosVenda;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ListarPedidosVenda;

public static class ListarPedidosVendaMap
{
    public static ListarPedidosVendaQuery ToQuery(this ListarPedidosVendaRequest request)
        => new(request.Status, request.ClienteId, request.VendedorId,
            request.Inicio, request.Fim, request.Skip, request.Take);

    public static ListarPedidosVendaResponse ToResponse(this ListarPedidosVendaQueryResult result)
        => new(
            result.Items.Select(i => i.ToResponseItem()).ToArray(),
            result.Total, result.Skip, result.Take);

    private static ListarPedidosVendaResponseItem ToResponseItem(this ListarPedidosVendaQueryItem item)
        => new(item.Id, item.Numero,
            item.ClienteId, item.ClienteNome,
            item.VendedorId, item.VendedorNome,
            item.DataEmissao, item.ValorTotal, item.Status);
}
