using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

public sealed record ObterFluxoQuery(
    DateTime Inicio,
    DateTime Fim,
    bool SomenteRealizados = false) : IRequest<ResponseDefault<ObterFluxoQueryResult>>;
