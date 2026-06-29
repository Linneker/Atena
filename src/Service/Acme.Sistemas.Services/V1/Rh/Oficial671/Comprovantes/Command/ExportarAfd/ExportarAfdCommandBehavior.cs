using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAfd;

public sealed class ExportarAfdCommandBehavior
    : IPipelineBehavior<ExportarAfdCommand, ResponseDefault<ExportarAfdCommandResult>>
{
    public Task<ResponseDefault<ExportarAfdCommandResult>> Handle(
        ExportarAfdCommand request,
        RequestHandlerDelegate<ResponseDefault<ExportarAfdCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
