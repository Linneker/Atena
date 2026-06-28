using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ListarPedidosVenda;

public sealed record ListarPedidosVendaQueryItem(
    Guid Id,
    string Numero,
    Guid ClienteId,
    string? ClienteNome,
    Guid? VendedorId,
    string? VendedorNome,
    DateTime DataEmissao,
    decimal ValorTotal,
    StatusPedidoVenda Status);

public sealed record ListarPedidosVendaQueryResult(
    IReadOnlyList<ListarPedidosVendaQueryItem> Items,
    long Total,
    int Skip,
    int Take);
