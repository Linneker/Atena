using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface ILotacaoRepository : IBaseRepository<Lotacao>
{
    Task<Lotacao?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
}
