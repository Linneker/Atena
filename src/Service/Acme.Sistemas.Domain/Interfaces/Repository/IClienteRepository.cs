using Acme.Sistemas.Domain.Entities.Cadastros;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IClienteRepository : IBaseRepository<Cliente>
{
    Task<Cliente?> GetByDocumentoAsync(string documento, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Cliente>> ListByFiltroAsync(
        string? termo, bool? inadimplente, int skip, int take,
        CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(string? termo, bool? inadimplente, CancellationToken cancellationToken = default);
    Task UpdateInadimplenciaAsync(Guid id, bool inadimplente, bool bloqueadoVendas, CancellationToken cancellationToken = default);

    /// <summary>Batch lookup id → nome, escopado por tenant. Retorna dicionário vazio se ids vazio.</summary>
    Task<IReadOnlyDictionary<Guid, string>> GetNomesByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}
