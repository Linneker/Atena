using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Espelho.Query.ObterEspelhoMensal;

public sealed class ObterEspelhoMensalQueryBehavior
    : IPipelineBehavior<ObterEspelhoMensalQuery, ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>>
{
    public Task<ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>> Handle(
        ObterEspelhoMensalQuery request,
        RequestHandlerDelegate<ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>> next,
        CancellationToken cancellationToken) => next();
}
