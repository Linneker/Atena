using Acme.Sistemas.Services.V1.PedidoCompra.Query.ObterPedidoCompra;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.ObterPedidoCompra;

public static class ObterPedidoCompraMap
{
    public static ObterPedidoCompraQuery ToQuery(this ObterPedidoCompraRequest request) => new(request.Id);

    public static ObterPedidoCompraResponse ToResponse(this ObterPedidoCompraQueryResult r)
        => new(r.Id, r.Numero,
            r.FornecedorId, r.FornecedorNome,
            r.DataEmissao, r.PrevisaoEntrega,
            r.ValorTotal, r.Status,
            r.CondicaoPagamento, r.Observacao,
            r.Itens.Select(i => new ObterPedidoCompraResponseItem(
                i.Id, i.ProdutoId, i.ProdutoNome,
                i.Quantidade, i.QuantidadeRecebida, i.QuantidadePendente,
                i.PrecoUnitario, i.Total)).ToArray());
}
