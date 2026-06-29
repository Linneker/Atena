using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAej;

public sealed class ExportarAejCommandBehavior
    : IPipelineBehavior<ExportarAejCommand, ResponseDefault<ExportarAejCommandResult>>
{
    public Task<ResponseDefault<ExportarAejCommandResult>> Handle(
        ExportarAejCommand request,
        RequestHandlerDelegate<ResponseDefault<ExportarAejCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
