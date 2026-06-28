using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IAjustePontoRepository : IBaseRepository<AjustePonto>
{
    Task<IReadOnlyList<AjustePonto>> ListarPendentesAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountPendentesAsync(CancellationToken cancellationToken = default);
}
