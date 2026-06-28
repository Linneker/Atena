using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.CriarJornada;

public sealed class CriarJornadaCommandBehavior
    : IPipelineBehavior<CriarJornadaCommand, ResponseDefault<CriarJornadaCommandResult>>
{
    public Task<ResponseDefault<CriarJornadaCommandResult>> Handle(
        CriarJornadaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarJornadaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
