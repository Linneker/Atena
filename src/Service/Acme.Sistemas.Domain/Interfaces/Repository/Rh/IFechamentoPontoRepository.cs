using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IFechamentoPontoRepository : IBaseRepository<FechamentoPonto>
{
    Task<FechamentoPonto?> GetByFuncionarioCompetenciaAsync(Guid funcionarioId, string competencia, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FechamentoPonto>> ListByCompetenciaAsync(string competencia, CancellationToken cancellationToken = default);
}
