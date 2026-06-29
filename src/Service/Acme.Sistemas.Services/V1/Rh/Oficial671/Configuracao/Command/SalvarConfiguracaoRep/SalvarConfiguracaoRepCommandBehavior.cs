using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Command.SalvarConfiguracaoRep;

public sealed class SalvarConfiguracaoRepCommandBehavior
    : IPipelineBehavior<SalvarConfiguracaoRepCommand, ResponseDefault<SalvarConfiguracaoRepCommandResult>>
{
    public Task<ResponseDefault<SalvarConfiguracaoRepCommandResult>> Handle(
        SalvarConfiguracaoRepCommand request,
        RequestHandlerDelegate<ResponseDefault<SalvarConfiguracaoRepCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
