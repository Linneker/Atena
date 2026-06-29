using Acme.Sistemas.Domain.Entities.Rh.Oficial671;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IConfiguracaoRepRepository : IBaseRepository<ConfiguracaoRep>
{
    Task<ConfiguracaoRep?> GetByEmpresaAsync(Guid empresaId, CancellationToken cancellationToken = default);
}
