using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.ConfirmarPedidoVenda;

public sealed record ConfirmarPedidoVendaCommand(Guid Id) : IRequest<ResponseDefault<ConfirmarPedidoVendaCommandResult>>;

public sealed record ConfirmarPedidoVendaCommandResult(Guid Id, decimal QuantidadeReservada);
