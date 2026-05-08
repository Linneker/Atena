using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

public sealed record PosicaoEstoqueLinhaView(
    Guid ProdutoId, string CodigoProduto, string NomeProduto,
    decimal SaldoTotal, decimal SaldoReservado, decimal SaldoDisponivel,
    decimal? CustoMedio, decimal ValorEstoque);

public sealed record PosicaoEstoqueQueryResult(
    Guid? EstoqueId,
    int TotalProdutos,
    decimal ValorTotalEstoque,
    IReadOnlyList<PosicaoEstoqueLinhaView> Linhas);
