using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;

public sealed record ListarJornadasQuery(
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarJornadasQueryResult>>;
