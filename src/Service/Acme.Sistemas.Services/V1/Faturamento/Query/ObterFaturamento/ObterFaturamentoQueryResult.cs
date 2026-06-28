using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ObterFaturamento;

public sealed record ObterFaturamentoItem(
    Guid Id,
    Guid PedidoVendaItemId,
    Guid ProdutoId,
    string? ProdutoNome,
    decimal Quantidade,
    decimal PrecoUnitario,
    decimal Total);

public sealed record ObterFaturamentoQueryResult(
    Guid Id,
    string Numero,
    Guid PedidoVendaId,
    DateTime DataFaturamento,
    TipoFaturamento Tipo,
    decimal ValorTotal,
    Guid? NFeId,
    Guid? ContaReceberId,
    string? Observacao,
    IReadOnlyList<ObterFaturamentoItem> Itens);
