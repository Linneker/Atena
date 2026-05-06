using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IPlanoDeContasRepository : IBaseRepository<PlanoDeContas>
{
    Task<PlanoDeContas?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PlanoDeContas>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PlanoDeContas>> ListFilhosAsync(Guid paiId, CancellationToken cancellationToken = default);
    Task<bool> HasFilhosAsync(Guid paiId, CancellationToken cancellationToken = default);
}
