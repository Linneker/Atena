using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ObterPedidoVenda;

public sealed record ObterPedidoVendaItem(
    Guid Id,
    Guid ProdutoId,
    string? ProdutoNome,
    decimal Quantidade,
    decimal QuantidadeFaturada,
    decimal QuantidadePendente,
    decimal PrecoUnitario,
    decimal Total);

public sealed record ObterPedidoVendaQueryResult(
    Guid Id,
    string Numero,
    Guid ClienteId,
    string? ClienteNome,
    Guid? VendedorId,
    Guid? OrcamentoId,
    DateTime DataEmissao,
    Guid EstoqueId,
    decimal ValorTotal,
    decimal? DescontoPercentual,
    StatusPedidoVenda Status,
    string? CondicaoPagamento,
    string? Observacao,
    IReadOnlyList<ObterPedidoVendaItem> Itens);
