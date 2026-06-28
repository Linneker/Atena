using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IEscalaFuncionarioRepository : IBaseRepository<EscalaFuncionario>
{
    Task<IReadOnlyList<EscalaFuncionario>> ListByFuncionarioAsync(Guid funcionarioId, CancellationToken cancellationToken = default);
    Task<EscalaFuncionario?> GetVigenteAsync(Guid funcionarioId, DateOnly em, CancellationToken cancellationToken = default);
    Task FecharVigenciaAsync(Guid id, DateOnly vigenciaFim, Guid? updatedBy, CancellationToken cancellationToken = default);
}
