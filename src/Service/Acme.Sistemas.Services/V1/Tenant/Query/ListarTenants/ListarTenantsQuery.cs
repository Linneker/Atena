using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

public sealed record ListarTenantsQuery(int Skip = 0, int Take = 50)
    : IRequest<ResponseDefault<ListarTenantsQueryResult>>;
