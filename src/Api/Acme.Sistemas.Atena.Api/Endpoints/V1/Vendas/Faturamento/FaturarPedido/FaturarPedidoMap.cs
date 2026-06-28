using Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.FaturarPedido;

public static class FaturarPedidoMap
{
    public static FaturarPedidoCommand ToCommand(this FaturarPedidoRequest request)
        => new(
            request.PedidoVendaId,
            request.VencimentoContaReceber,
            request.PlanoDeContasId,
            request.PercentualComissaoOverride,
            request.Itens.Select(i => new FaturarPedidoItemDto(i.PedidoVendaItemId, i.Quantidade)).ToArray());

    public static FaturarPedidoResponse ToResponse(this FaturarPedidoCommandResult result)
        => new(
            result.FaturamentoId,
            result.Numero,
            result.ValorTotal,
            result.ContaReceberId,
            result.ComissaoId,
            result.NFeSolicitada);
}
