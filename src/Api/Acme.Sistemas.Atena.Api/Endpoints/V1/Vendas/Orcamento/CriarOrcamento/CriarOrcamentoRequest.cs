namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.CriarOrcamento;

public sealed record CriarOrcamentoRequestItem(
    Guid ProdutoId,
    decimal Quantidade,
    decimal PrecoUnitario);

public sealed record CriarOrcamentoRequest(
    Guid ClienteId,
    Guid? VendedorId,
    DateTime DataValidade,
    decimal? DescontoPercentual,
    string? Observacao,
    IReadOnlyList<CriarOrcamentoRequestItem> Itens);
