using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;

public sealed record FaturarPedidoItemDto(Guid PedidoVendaItemId, decimal Quantidade);

