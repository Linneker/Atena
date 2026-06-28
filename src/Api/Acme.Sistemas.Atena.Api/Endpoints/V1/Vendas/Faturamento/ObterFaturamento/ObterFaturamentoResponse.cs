using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.ObterFaturamento;

public sealed record ObterFaturamentoResponseItem(
    Guid Id,
    Guid PedidoVendaItemId,
    Guid ProdutoId,
    string? ProdutoNome,
    decimal Quantidade,
    decimal PrecoUnitario,
    decimal Total);

public sealed record ObterFaturamentoResponse(
    Guid Id,
    string Numero,
    Guid PedidoVendaId,
    DateTime DataFaturamento,
    TipoFaturamento Tipo,
    decimal ValorTotal,
    Guid? NFeId,
    Guid? ContaReceberId,
    string? Observacao,
    IReadOnlyList<ObterFaturamentoResponseItem> Itens);
