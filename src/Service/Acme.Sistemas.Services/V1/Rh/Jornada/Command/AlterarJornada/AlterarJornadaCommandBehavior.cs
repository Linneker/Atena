using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;

public sealed class AlterarJornadaCommandBehavior
    : IPipelineBehavior<AlterarJornadaCommand, ResponseDefault<AlterarJornadaCommandResult>>
{
    public Task<ResponseDefault<AlterarJornadaCommandResult>> Handle(
        AlterarJornadaCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarJornadaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
