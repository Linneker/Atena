using Acme.Sistemas.Domain.Entities.Rh.Oficial671;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IExportacaoAfdRepository : IBaseRepository<ExportacaoAfd>
{
    Task<IReadOnlyList<ExportacaoAfd>> ListByEmpresaAsync(
        Guid empresaId, CancellationToken cancellationToken = default);
}
