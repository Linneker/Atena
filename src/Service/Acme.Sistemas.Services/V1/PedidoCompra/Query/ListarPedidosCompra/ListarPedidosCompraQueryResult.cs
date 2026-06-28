using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ListarPedidosCompra;

public sealed record ListarPedidosCompraQueryItem(
    Guid Id,
    string Numero,
    Guid FornecedorId,
    string? FornecedorNome,
    DateTime DataEmissao,
    DateTime? PrevisaoEntrega,
    decimal ValorTotal,
    StatusPedidoCompra Status);

public sealed record ListarPedidosCompraQueryResult(
    IReadOnlyList<ListarPedidosCompraQueryItem> Items,
    long Total,
    int Skip,
    int Take);
