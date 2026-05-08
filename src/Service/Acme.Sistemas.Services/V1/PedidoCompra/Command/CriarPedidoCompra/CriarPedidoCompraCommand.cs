using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;

public sealed record PedidoCompraItemDto(Guid ProdutoId, decimal Quantidade, decimal PrecoUnitario);

