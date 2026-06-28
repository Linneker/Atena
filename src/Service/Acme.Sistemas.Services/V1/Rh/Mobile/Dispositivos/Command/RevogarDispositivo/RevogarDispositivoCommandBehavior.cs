using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RevogarDispositivo;

public sealed class RevogarDispositivoCommandBehavior
    : IPipelineBehavior<RevogarDispositivoCommand, ResponseDefault<RevogarDispositivoCommandResult>>
{
    public Task<ResponseDefault<RevogarDispositivoCommandResult>> Handle(
        RevogarDispositivoCommand request,
        RequestHandlerDelegate<ResponseDefault<RevogarDispositivoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
