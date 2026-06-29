using Acme.Sistemas.Domain.Entities.Rh.Oficial671;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IExportacaoAejRepository : IBaseRepository<ExportacaoAej>
{
    Task<IReadOnlyList<ExportacaoAej>> ListByEmpresaAsync(
        Guid empresaId, CancellationToken cancellationToken = default);
}
