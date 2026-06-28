using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ICentroDeCustoRepository : IBaseRepository<CentroDeCusto>
{
    Task<CentroDeCusto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<long> CountVinculosAsync(Guid centroId, CancellationToken cancellationToken = default);

    /// <summary>Batch lookup id → nome, escopado por tenant. Retorna dicionário vazio se ids vazio.</summary>
    Task<IReadOnlyDictionary<Guid, string>> GetNomesByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}
