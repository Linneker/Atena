using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ListarEstoques;

public sealed record ListarEstoquesQuery(int Skip = 0, int Take = 100) : IRequest<ResponseDefault<ListarEstoquesQueryResult>>;
