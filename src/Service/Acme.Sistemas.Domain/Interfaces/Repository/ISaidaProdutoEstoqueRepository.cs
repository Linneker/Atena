using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ISaidaProdutoEstoqueRepository : IBaseRepository<SaidaProdutoEstoque>
{
    Task<IReadOnlyList<SaidaProdutoEstoque>> ListByProdutoAsync(
        Guid produtoId, DateTime? inicio, DateTime? fim, int skip, int take,
        CancellationToken cancellationToken = default);
}
