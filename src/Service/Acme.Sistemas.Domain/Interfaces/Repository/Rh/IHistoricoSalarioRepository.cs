using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IHistoricoSalarioRepository : IBaseRepository<HistoricoSalario>
{
    Task<IReadOnlyList<HistoricoSalario>> ListByFuncionarioAsync(Guid funcionarioId, CancellationToken cancellationToken = default);
    Task<HistoricoSalario?> GetVigenteAsync(Guid funcionarioId, DateOnly em, CancellationToken cancellationToken = default);
    Task FecharVigenciaAsync(Guid id, DateOnly vigenciaFim, Guid? updatedBy, CancellationToken cancellationToken = default);
}
