using Acme.Sistemas.Domain.Entities.Referencia;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ICodigoServicoLc116Repository
{
    Task<IReadOnlyList<CodigoServicoLc116>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<long> CountAsync(CancellationToken cancellationToken = default);
}
