using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ObterPedidoCompra;

public sealed record ObterPedidoCompraItem(
    Guid Id,
    Guid ProdutoId,
    string? ProdutoNome,
    decimal Quantidade,
    decimal QuantidadeRecebida,
    decimal QuantidadePendente,
    decimal PrecoUnitario,
    decimal Total);

public sealed record ObterPedidoCompraQueryResult(
    Guid Id,
    string Numero,
    Guid FornecedorId,
    string? FornecedorNome,
    DateTime DataEmissao,
    DateTime? PrevisaoEntrega,
    decimal ValorTotal,
    StatusPedidoCompra Status,
    string? CondicaoPagamento,
    string? Observacao,
    IReadOnlyList<ObterPedidoCompraItem> Itens);
