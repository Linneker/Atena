namespace Acme.Sistemas.Domain.Interfaces.Repository;

public sealed record PosicaoEstoqueLinha(
    Guid ProdutoId,
    string CodigoProduto,
    string NomeProduto,
    decimal SaldoTotal,
    decimal SaldoReservado,
    decimal SaldoDisponivel,
    decimal? CustoMedio,
    decimal ValorEstoque);

public interface IPosicaoEstoqueRepository
{
    Task<IReadOnlyList<PosicaoEstoqueLinha>> ConsultarAsync(Guid? estoqueId, CancellationToken cancellationToken = default);
}
