using Acme.Sistemas.Domain.Entities.Rh.Oficial671;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IComprovantePontoRepository : IBaseRepository<ComprovantePonto>
{
    Task<ComprovantePonto?> GetByMarcacaoAsync(Guid marcacaoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComprovantePonto>> ListByEmpresaPeriodoAsync(
        Guid empresaId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}
