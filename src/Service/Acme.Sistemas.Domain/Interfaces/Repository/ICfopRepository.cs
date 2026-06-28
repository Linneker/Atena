using Acme.Sistemas.Domain.Entities.Referencia;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ICfopRepository
{
    Task<IReadOnlyList<Cfop>> ListAsync(string? categoria, CancellationToken cancellationToken = default);
    Task<long> CountAsync(CancellationToken cancellationToken = default);
}
