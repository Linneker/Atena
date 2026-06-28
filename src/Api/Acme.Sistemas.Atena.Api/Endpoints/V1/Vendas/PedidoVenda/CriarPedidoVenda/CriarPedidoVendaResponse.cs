namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.CriarPedidoVenda;

public sealed record CriarPedidoVendaResponse(
    Guid Id,
    string Numero,
    decimal ValorTotal);
