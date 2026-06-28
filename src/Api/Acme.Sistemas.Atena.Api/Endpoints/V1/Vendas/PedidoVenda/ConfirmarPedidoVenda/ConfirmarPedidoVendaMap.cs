using Acme.Sistemas.Services.V1.PedidoVenda.Command.ConfirmarPedidoVenda;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ConfirmarPedidoVenda;

public static class ConfirmarPedidoVendaMap
{
    public static ConfirmarPedidoVendaCommand ToCommand(this ConfirmarPedidoVendaRequest request)
        => new(request.Id);

    public static ConfirmarPedidoVendaResponse ToResponse(this ConfirmarPedidoVendaCommandResult result)
        => new(result.Id, result.QuantidadeReservada);
}
