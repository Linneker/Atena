using Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.CriarPedidoCompra;

public static class CriarPedidoCompraMap
{
    public static CriarPedidoCompraCommand ToCommand(this CriarPedidoCompraRequest request)
        => new(
            request.FornecedorId,
            request.SolicitacaoCompraId,
            request.PrevisaoEntrega,
            request.CondicaoPagamento,
            request.Observacao,
            request.Itens?.Select(i => new PedidoCompraItemDto(i.ProdutoId, i.Quantidade, i.PrecoUnitario)).ToArray());

    public static CriarPedidoCompraResponse ToResponse(this CriarPedidoCompraCommandResult result)
        => new(result.Id, result.Numero, result.ValorTotal);
}
