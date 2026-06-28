namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.FaturarPedido;

public sealed record FaturarPedidoRequestItem(
    Guid PedidoVendaItemId,
    decimal Quantidade);

public sealed record FaturarPedidoRequest(
    Guid PedidoVendaId,
    DateTime VencimentoContaReceber,
    Guid? PlanoDeContasId,
    decimal? PercentualComissaoOverride,
    IReadOnlyList<FaturarPedidoRequestItem> Itens);
