using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IEntradaProdutoEstoqueRepository : IBaseRepository<EntradaProdutoEstoque>
{
    Task<IReadOnlyList<EntradaProdutoEstoque>> ListByProdutoAsync(
        Guid produtoId, DateTime? inicio, DateTime? fim, int skip, int take,
        CancellationToken cancellationToken = default);

    /// <summary>Lotes com saldo restante (FIFO — mais antigos primeiro).</summary>
    Task<IReadOnlyList<EntradaProdutoEstoque>> ListLotesAbertosFifoAsync(
        Guid estoqueId, Guid produtoId, CancellationToken cancellationToken = default);

    Task ConsumirLoteAsync(Guid loteId, decimal quantidade, CancellationToken cancellationToken = default);
}
