using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

public sealed record ListarUsuariosQuery(int Skip = 0, int Take = 50)
    : IRequest<ResponseDefault<ListarUsuariosQueryResult>>;
