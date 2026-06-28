using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IBeneficioCatalogoRepository : IBaseRepository<BeneficioCatalogo>
{
    Task<BeneficioCatalogo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
