using Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ListarTenants;

public static class ListarTenantsMap
{
    public static ListarTenantsQuery ToQuery(this ListarTenantsRequest request)
        => new(request.Skip, request.Take);

    public static ListarTenantsResponse ToResponse(this ListarTenantsQueryResult result)
        => new(
            result.Items.Select(i => new ListarTenantsResponseItem(i.Id, i.RazaoSocial, i.Cnpj, i.Plano, i.Status, i.CreatedAt)).ToArray(),
            result.Total);
}
