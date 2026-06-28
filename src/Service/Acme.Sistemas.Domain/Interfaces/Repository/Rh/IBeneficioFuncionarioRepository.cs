using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IBeneficioFuncionarioRepository : IBaseRepository<BeneficioFuncionario>
{
    Task<IReadOnlyList<BeneficioFuncionario>> ListByFuncionarioAsync(Guid funcionarioId, CancellationToken cancellationToken = default);
    Task<BeneficioFuncionario?> GetVigenteAsync(Guid funcionarioId, Guid beneficioCatalogoId, DateOnly em, CancellationToken cancellationToken = default);
}
