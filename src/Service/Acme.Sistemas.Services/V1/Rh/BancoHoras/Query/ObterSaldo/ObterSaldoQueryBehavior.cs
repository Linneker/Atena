using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ObterSaldo;

public sealed class ObterSaldoQueryBehavior
    : IPipelineBehavior<ObterSaldoQuery, ResponseDefault<ObterSaldoQueryResult>>
{
    public Task<ResponseDefault<ObterSaldoQueryResult>> Handle(
        ObterSaldoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterSaldoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
