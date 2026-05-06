using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IConfiguracaoFiscalRepository
{
    Task<ConfiguracaoFiscal?> GetAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(ConfiguracaoFiscal config, CancellationToken cancellationToken = default);
    Task<int> ReservarProximoNumeroAsync(int serie, CancellationToken cancellationToken = default);
}
