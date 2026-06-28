using Acme.Sistemas.Domain.Entities.Rh;

namespace Acme.Sistemas.Domain.Interfaces.Repository.Rh;

public interface IDispositivoMobileRepository : IBaseRepository<DispositivoMobile>
{
    Task<DispositivoMobile?> GetByDeviceIdAsync(Guid usuarioId, string deviceId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DispositivoMobile>> ListByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DispositivoMobile>> ListAllTenantAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountTenantAsync(CancellationToken cancellationToken = default);
    Task RevogarAsync(Guid id, Guid revogadoPor, CancellationToken cancellationToken = default);
    Task RegistrarUltimoAcessoAsync(Guid id, CancellationToken cancellationToken = default);
}
