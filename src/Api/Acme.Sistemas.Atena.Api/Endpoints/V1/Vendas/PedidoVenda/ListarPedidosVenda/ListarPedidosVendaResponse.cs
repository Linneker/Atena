using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ListarPedidosVenda;

public sealed record ListarPedidosVendaResponseItem(
    Guid Id,
    string Numero,
    Guid ClienteId,
    string? ClienteNome,
    Guid? VendedorId,
    string? VendedorNome,
    DateTime DataEmissao,
    decimal ValorTotal,
    StatusPedidoVenda Status);

public sealed record ListarPedidosVendaResponse(
    IReadOnlyList<ListarPedidosVendaResponseItem> Items,
    long Total,
    int Skip,
    int Take);
