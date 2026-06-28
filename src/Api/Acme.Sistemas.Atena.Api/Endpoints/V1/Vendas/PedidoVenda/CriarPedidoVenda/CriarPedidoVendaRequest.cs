namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.CriarPedidoVenda;

public sealed record CriarPedidoVendaRequestItem(
    Guid ProdutoId,
    decimal Quantidade,
    decimal PrecoUnitario);

public sealed record CriarPedidoVendaRequest(
    Guid ClienteId,
    Guid? VendedorId,
    Guid EstoqueId,
    Guid? OrcamentoId,
    decimal? DescontoPercentual,
    string? CondicaoPagamento,
    string? Observacao,
    IReadOnlyList<CriarPedidoVendaRequestItem> Itens);
