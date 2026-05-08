namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.PosicaoEstoque.ObterPosicaoEstoque;

public sealed record PosicaoEstoqueLinhaResponse(
    Guid ProdutoId,
    string CodigoProduto,
    string NomeProduto,
    decimal SaldoTotal,
    decimal SaldoReservado,
    decimal SaldoDisponivel,
    decimal? CustoMedio,
    decimal ValorEstoque);

public sealed record ObterPosicaoEstoqueResponse(
    Guid? EstoqueId,
    int TotalProdutos,
    decimal ValorTotalEstoque,
    IReadOnlyList<PosicaoEstoqueLinhaResponse> Linhas);
