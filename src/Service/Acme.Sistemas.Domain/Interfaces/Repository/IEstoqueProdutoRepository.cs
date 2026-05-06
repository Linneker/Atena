using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IEstoqueProdutoRepository : IBaseRepository<EstoqueProduto>
{
    Task<EstoqueProduto?> GetByEstoqueAndProdutoAsync(Guid estoqueId, Guid produtoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EstoqueProduto>> ListByProdutoAsync(Guid produtoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EstoqueProduto>> ListByEstoqueAsync(Guid estoqueId, int skip, int take, CancellationToken cancellationToken = default);
    Task UpsertSaldoAsync(EstoqueProduto saldo, CancellationToken cancellationToken = default);
    Task AjustarSaldoAsync(Guid estoqueId, Guid produtoId, decimal deltaTotal, decimal deltaReservado, CancellationToken cancellationToken = default);
}
