using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.ListarPedidosCompra;

public sealed record ListarPedidosCompraResponseItem(
    Guid Id,
    string Numero,
    Guid FornecedorId,
    string? FornecedorNome,
    DateTime DataEmissao,
    DateTime? PrevisaoEntrega,
    decimal ValorTotal,
    StatusPedidoCompra Status);

public sealed record ListarPedidosCompraResponse(
    IReadOnlyList<ListarPedidosCompraResponseItem> Items,
    long Total,
    int Skip,
    int Take);
