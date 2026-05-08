using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarRoles;

public sealed record ListarRolesQuery(int Skip = 0, int Take = 50)
    : IRequest<ResponseDefault<ListarRolesQueryResult>>;

