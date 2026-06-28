using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IFornecedorRepository : IBaseRepository<Fornecedor>
{
    Task<Fornecedor?> GetByDocumentoAsync(string documento, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Fornecedor>> ListByFiltroAsync(
        string? termo, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(string? termo, CancellationToken cancellationToken = default);

    /// <summary>Batch lookup id → nome, escopado por tenant. Retorna dicionário vazio se ids vazio.</summary>
    Task<IReadOnlyDictionary<Guid, string>> GetNomesByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}
