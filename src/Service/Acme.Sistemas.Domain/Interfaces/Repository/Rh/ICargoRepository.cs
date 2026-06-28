using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface ICargoRepository : IBaseRepository<Cargo>
{
    Task<Cargo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
