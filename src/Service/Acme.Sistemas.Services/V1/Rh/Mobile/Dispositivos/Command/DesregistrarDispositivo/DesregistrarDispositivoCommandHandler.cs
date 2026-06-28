using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.DesregistrarDispositivo;

public sealed class DesregistrarDispositivoCommandHandler
    : IRequestHandler<DesregistrarDispositivoCommand, ResponseDefault<DesregistrarDispositivoCommandResult>>
{
    private readonly IDispositivoMobileRepository _repo;
    private readonly ITenantContext _tenantContext;

    public DesregistrarDispositivoCommandHandler(IDispositivoMobileRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<DesregistrarDispositivoCommandResult>> Handle(
        DesregistrarDispositivoCommand request, CancellationToken cancellationToken)
    {
        var userId = _tenantContext.UserId
            ?? throw new InvalidOperationException("UserId obrigatório.");

        var existente = await _repo.GetByDeviceIdAsync(userId, request.DeviceId, cancellationToken);
        if (existente is null)
            return ResponseDefault<DesregistrarDispositivoCommandResult>.NotFound(
                $"Dispositivo {request.DeviceId} não encontrado para este usuário.");

        await _repo.RevogarAsync(existente.Id, userId, cancellationToken);
        return ResponseDefault<DesregistrarDispositivoCommandResult>.Ok(
            new DesregistrarDispositivoCommandResult(existente.Id));
    }
}
