using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Divida.Query.ListarDividas;

public sealed record ListarDividasQuery(
    StatusConta? Status = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarDividasQueryResult>>;

