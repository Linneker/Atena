using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IDependenteRepository : IBaseRepository<Dependente>
{
    Task<IReadOnlyList<Dependente>> ListByFuncionarioAsync(Guid funcionarioId, CancellationToken cancellationToken = default);
}
