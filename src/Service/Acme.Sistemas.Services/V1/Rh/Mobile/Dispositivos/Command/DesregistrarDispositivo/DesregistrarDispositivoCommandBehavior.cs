using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.DesregistrarDispositivo;

public sealed class DesregistrarDispositivoCommandBehavior
    : IPipelineBehavior<DesregistrarDispositivoCommand, ResponseDefault<DesregistrarDispositivoCommandResult>>
{
    public Task<ResponseDefault<DesregistrarDispositivoCommandResult>> Handle(
        DesregistrarDispositivoCommand request,
        RequestHandlerDelegate<ResponseDefault<DesregistrarDispositivoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
