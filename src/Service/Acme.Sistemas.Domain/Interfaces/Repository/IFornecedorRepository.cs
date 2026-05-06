using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IFornecedorRepository : IBaseRepository<Fornecedor>
{
    Task<Fornecedor?> GetByDocumentoAsync(string documento, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Fornecedor>> ListByFiltroAsync(
        string? termo, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(string? termo, CancellationToken cancellationToken = default);
}
