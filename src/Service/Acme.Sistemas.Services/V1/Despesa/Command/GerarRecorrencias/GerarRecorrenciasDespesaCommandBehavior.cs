using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;

public sealed class GerarRecorrenciasDespesaCommandBehavior
    : IPipelineBehavior<GerarRecorrenciasDespesaCommand, ResponseDefault<GerarRecorrenciasDespesaCommandResult>>
{
    public Task<ResponseDefault<GerarRecorrenciasDespesaCommandResult>> Handle(
        GerarRecorrenciasDespesaCommand request,
        RequestHandlerDelegate<ResponseDefault<GerarRecorrenciasDespesaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
