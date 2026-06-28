using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IDepartamentoRepository : IBaseRepository<Departamento>
{
    Task<Departamento?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
