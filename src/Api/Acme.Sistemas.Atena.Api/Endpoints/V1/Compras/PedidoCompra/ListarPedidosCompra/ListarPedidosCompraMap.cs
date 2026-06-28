using Acme.Sistemas.Services.V1.PedidoCompra.Query.ListarPedidosCompra;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.ListarPedidosCompra;

public static class ListarPedidosCompraMap
{
    public static ListarPedidosCompraQuery ToQuery(this ListarPedidosCompraRequest request)
        => new(request.Status, request.FornecedorId, request.Skip, request.Take);

    public static ListarPedidosCompraResponse ToResponse(this ListarPedidosCompraQueryResult result)
        => new(
            result.Items.Select(i => i.ToResponseItem()).ToArray(),
            result.Total, result.Skip, result.Take);

    private static ListarPedidosCompraResponseItem ToResponseItem(this ListarPedidosCompraQueryItem item)
        => new(item.Id, item.Numero, item.FornecedorId, item.FornecedorNome,
            item.DataEmissao, item.PrevisaoEntrega, item.ValorTotal, item.Status);
}
