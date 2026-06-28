using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.RemoverLotacao;

public sealed class RemoverLotacaoCommandBehavior
    : IPipelineBehavior<RemoverLotacaoCommand, ResponseDefault<RemoverLotacaoCommandResult>>
{
    public Task<ResponseDefault<RemoverLotacaoCommandResult>> Handle(
        RemoverLotacaoCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverLotacaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
