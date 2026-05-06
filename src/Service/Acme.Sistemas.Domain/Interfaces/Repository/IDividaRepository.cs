using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IDividaRepository : IBaseRepository<Divida>
{
    Task<IReadOnlyList<Divida>> ListByFiltroAsync(StatusConta? status, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(StatusConta? status, CancellationToken cancellationToken = default);
}
