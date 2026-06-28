using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.CriarLotacao;

public sealed class CriarLotacaoCommandBehavior
    : IPipelineBehavior<CriarLotacaoCommand, ResponseDefault<CriarLotacaoCommandResult>>
{
    public Task<ResponseDefault<CriarLotacaoCommandResult>> Handle(
        CriarLotacaoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarLotacaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
