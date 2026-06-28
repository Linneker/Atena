using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RevogarDispositivo;

public sealed class RevogarDispositivoCommandHandler
    : IRequestHandler<RevogarDispositivoCommand, ResponseDefault<RevogarDispositivoCommandResult>>
{
    private readonly IDispositivoMobileRepository _repo;
    private readonly ITenantContext _tenantContext;

    public RevogarDispositivoCommandHandler(IDispositivoMobileRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RevogarDispositivoCommandResult>> Handle(
        RevogarDispositivoCommand request, CancellationToken cancellationToken)
    {
        var disp = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (disp is null)
            return ResponseDefault<RevogarDispositivoCommandResult>.NotFound(
                $"Dispositivo {request.Id} não encontrado.");

        var revogadoPor = _tenantContext.UserId
            ?? throw new InvalidOperationException("UserId obrigatório.");

        await _repo.RevogarAsync(disp.Id, revogadoPor, cancellationToken);
        return ResponseDefault<RevogarDispositivoCommandResult>.Ok(
            new RevogarDispositivoCommandResult(disp.Id));
    }
}
