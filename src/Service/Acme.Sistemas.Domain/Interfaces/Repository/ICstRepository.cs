using Acme.Sistemas.Domain.Entities.Referencia;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ICstRepository
{
    /// <summary>Tipos suportados: icms, pis, cofins, ipi.</summary>
    Task<IReadOnlyList<Cst>> ListByTipoAsync(string tipo, CancellationToken cancellationToken = default);
    Task<long> CountAsync(string tipo, CancellationToken cancellationToken = default);
}
