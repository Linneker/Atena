using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IFechamentoPeriodoRepository : IBaseRepository<FechamentoPeriodo>
{
    Task<FechamentoPeriodo?> GetByPeriodoAsync(int ano, int mes, CancellationToken cancellationToken = default);
}
