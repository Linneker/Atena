using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPontoMobile;

public sealed class BaterPontoMobileCommandBehavior
    : IPipelineBehavior<BaterPontoMobileCommand, ResponseDefault<BaterPontoMobileCommandResult>>
{
    public Task<ResponseDefault<BaterPontoMobileCommandResult>> Handle(
        BaterPontoMobileCommand request,
        RequestHandlerDelegate<ResponseDefault<BaterPontoMobileCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
