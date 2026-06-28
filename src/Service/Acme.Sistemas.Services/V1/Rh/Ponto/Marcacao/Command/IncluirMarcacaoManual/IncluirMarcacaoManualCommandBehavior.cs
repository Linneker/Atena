using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.IncluirMarcacaoManual;

public sealed class IncluirMarcacaoManualCommandBehavior
    : IPipelineBehavior<IncluirMarcacaoManualCommand, ResponseDefault<IncluirMarcacaoManualCommandResult>>
{
    public Task<ResponseDefault<IncluirMarcacaoManualCommandResult>> Handle(
        IncluirMarcacaoManualCommand request,
        RequestHandlerDelegate<ResponseDefault<IncluirMarcacaoManualCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
