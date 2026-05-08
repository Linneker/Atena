using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;

public sealed record ObterKpisQuery(
    DateTime? Inicio = null,
    DateTime? Fim = null) : IRequest<ResponseDefault<ObterKpisQueryResult>>;

