using Acme.Sistemas.Services.V1.Tenant.Query.ObterTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ObterTenant;

public static class ObterTenantMap
{
    public static ObterTenantQuery ToQuery(this ObterTenantRequest request)
        => new(request.Id);

    public static ObterTenantResponse ToResponse(this ObterTenantQueryResult result)
        => new(
            result.Id,
            result.RazaoSocial,
            result.Cnpj,
            result.Plano,
            result.Status,
            result.LogoUrl,
            result.CorPrimaria,
            result.FusoHorario,
            result.CreatedAt);
}
