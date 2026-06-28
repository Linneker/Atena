using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ListarCargos;

public sealed record ListarCargosQuery(
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarCargosQueryResult>>;
