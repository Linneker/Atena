using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ICentroDeCustoRepository : IBaseRepository<CentroDeCusto>
{
    Task<CentroDeCusto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<long> CountVinculosAsync(Guid centroId, CancellationToken cancellationToken = default);
}
