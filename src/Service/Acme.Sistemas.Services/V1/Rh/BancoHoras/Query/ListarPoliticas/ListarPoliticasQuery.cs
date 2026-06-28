using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarPoliticas;

public sealed record ListarPoliticasQuery(int Skip = 0, int Take = 50)
    : IRequest<ResponseDefault<ListarPoliticasQueryResult>>;
