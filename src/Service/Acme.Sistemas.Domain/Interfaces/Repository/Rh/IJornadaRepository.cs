using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IJornadaRepository : IBaseRepository<Jornada>
{
    Task<Jornada?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
}
