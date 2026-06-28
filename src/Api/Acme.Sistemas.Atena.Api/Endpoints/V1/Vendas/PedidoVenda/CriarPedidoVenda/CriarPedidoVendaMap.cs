using Acme.Sistemas.Services.V1.PedidoVenda.Command.CriarPedidoVenda;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.CriarPedidoVenda;

public static class CriarPedidoVendaMap
{
    public static CriarPedidoVendaCommand ToCommand(this CriarPedidoVendaRequest request)
        => new(
            request.ClienteId,
            request.VendedorId,
            request.EstoqueId,
            request.OrcamentoId,
            request.DescontoPercentual,
            request.CondicaoPagamento,
            request.Observacao,
            request.Itens.Select(i => new PedidoVendaItemDto(i.ProdutoId, i.Quantidade, i.PrecoUnitario)).ToArray());

    public static CriarPedidoVendaResponse ToResponse(this CriarPedidoVendaCommandResult result)
        => new(result.Id, result.Numero, result.ValorTotal);
}
