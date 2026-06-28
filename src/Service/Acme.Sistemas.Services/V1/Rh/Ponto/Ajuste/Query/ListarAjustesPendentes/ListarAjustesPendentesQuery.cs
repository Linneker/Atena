using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Query.ListarAjustesPendentes;

public sealed record ListarAjustesPendentesQuery(int Skip = 0, int Take = 50)
    : IRequest<ResponseDefault<ListarAjustesPendentesQueryResult>>;
