using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.AlterarLotacao;

public sealed class AlterarLotacaoCommandBehavior
    : IPipelineBehavior<AlterarLotacaoCommand, ResponseDefault<AlterarLotacaoCommandResult>>
{
    public Task<ResponseDefault<AlterarLotacaoCommandResult>> Handle(
        AlterarLotacaoCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarLotacaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
