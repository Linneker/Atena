using Acme.Sistemas.Domain.Entities.Produtos;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IProdutoRepository : IBaseRepository<Produto>
{
    Task<Produto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<Produto?> GetByCodigoBarrasAsync(string codigoBarras, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Produto>> ListByFiltroAsync(string? termo, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(string? termo, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ValorProduto>> ListPrecosAsync(Guid produtoId, CancellationToken cancellationToken = default);
    Task UpsertPrecoAsync(ValorProduto preco, CancellationToken cancellationToken = default);
    Task ExpirarPrecosAtuaisAsync(Guid produtoId, Guid tipoValorProdutoId, DateTime dataFim, CancellationToken cancellationToken = default);
}
