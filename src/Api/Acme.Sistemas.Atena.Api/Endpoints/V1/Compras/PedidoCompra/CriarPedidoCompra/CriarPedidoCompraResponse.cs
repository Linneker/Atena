namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.CriarPedidoCompra;

public sealed record CriarPedidoCompraResponse(
    Guid Id,
    string Numero,
    decimal ValorTotal);
