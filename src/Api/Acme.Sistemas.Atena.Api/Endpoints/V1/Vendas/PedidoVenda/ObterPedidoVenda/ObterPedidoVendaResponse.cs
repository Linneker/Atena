using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ObterPedidoVenda;

public sealed record ObterPedidoVendaResponseItem(
    Guid Id,
    Guid ProdutoId,
    string? ProdutoNome,
    decimal Quantidade,
    decimal QuantidadeFaturada,
    decimal QuantidadePendente,
    decimal PrecoUnitario,
    decimal Total);

public sealed record ObterPedidoVendaResponse(
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
    IReadOnlyList<ObterPedidoVendaResponseItem> Itens);
