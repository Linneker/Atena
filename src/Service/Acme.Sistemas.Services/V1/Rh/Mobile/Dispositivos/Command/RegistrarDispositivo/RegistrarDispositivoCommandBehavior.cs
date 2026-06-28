using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RegistrarDispositivo;

public sealed class RegistrarDispositivoCommandBehavior
    : IPipelineBehavior<RegistrarDispositivoCommand, ResponseDefault<RegistrarDispositivoCommandResult>>
{
    public Task<ResponseDefault<RegistrarDispositivoCommandResult>> Handle(
        RegistrarDispositivoCommand request,
        RequestHandlerDelegate<ResponseDefault<RegistrarDispositivoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
