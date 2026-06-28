using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.GerarRecorrencias;

public sealed class GerarRecorrenciasReceitaCommandBehavior
    : IPipelineBehavior<GerarRecorrenciasReceitaCommand, ResponseDefault<GerarRecorrenciasReceitaCommandResult>>
{
    public Task<ResponseDefault<GerarRecorrenciasReceitaCommandResult>> Handle(
        GerarRecorrenciasReceitaCommand request,
        RequestHandlerDelegate<ResponseDefault<GerarRecorrenciasReceitaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
