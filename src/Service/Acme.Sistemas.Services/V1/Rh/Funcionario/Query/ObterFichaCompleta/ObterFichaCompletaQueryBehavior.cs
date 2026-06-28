using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ObterFichaCompleta;

public sealed class ObterFichaCompletaQueryBehavior
    : IPipelineBehavior<ObterFichaCompletaQuery, ResponseDefault<ObterFichaCompletaQueryResult>>
{
    public Task<ResponseDefault<ObterFichaCompletaQueryResult>> Handle(
        ObterFichaCompletaQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterFichaCompletaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
