using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

public sealed class BaterPontoCommandBehavior
    : IPipelineBehavior<BaterPontoCommand, ResponseDefault<BaterPontoCommandResult>>
{
    public Task<ResponseDefault<BaterPontoCommandResult>> Handle(
        BaterPontoCommand request,
        RequestHandlerDelegate<ResponseDefault<BaterPontoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
