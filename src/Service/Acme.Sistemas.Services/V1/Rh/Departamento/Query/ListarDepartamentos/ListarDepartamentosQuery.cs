using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ListarDepartamentos;

public sealed record ListarDepartamentosQuery(
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarDepartamentosQueryResult>>;
