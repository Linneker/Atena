using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.ObterPedidoCompra;

public sealed record ObterPedidoCompraResponseItem(
    Guid Id,
    Guid ProdutoId,
    string? ProdutoNome,
    decimal Quantidade,
    decimal QuantidadeRecebida,
    decimal QuantidadePendente,
    decimal PrecoUnitario,
    decimal Total);

public sealed record ObterPedidoCompraResponse(
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
    IReadOnlyList<ObterPedidoCompraResponseItem> Itens);
