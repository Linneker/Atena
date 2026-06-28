using Acme.Sistemas.Domain.Entities.Referencia;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IUfRepository
{
    Task<IReadOnlyList<Uf>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<long> CountAsync(CancellationToken cancellationToken = default);
}
