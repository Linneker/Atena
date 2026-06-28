using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;

public sealed class RemoverJornadaCommandBehavior
    : IPipelineBehavior<RemoverJornadaCommand, ResponseDefault<RemoverJornadaCommandResult>>
{
    public Task<ResponseDefault<RemoverJornadaCommandResult>> Handle(
        RemoverJornadaCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverJornadaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
