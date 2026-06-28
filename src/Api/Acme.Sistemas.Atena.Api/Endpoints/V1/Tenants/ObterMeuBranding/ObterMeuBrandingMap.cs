using Acme.Sistemas.Services.V1.Tenant.Query.ObterMeuBranding;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ObterMeuBranding;

public static class ObterMeuBrandingMap
{
    public static ObterMeuBrandingQuery ToQuery(this ObterMeuBrandingRequest _)
        => new();

    public static ObterMeuBrandingResponse ToResponse(this ObterMeuBrandingQueryResult result)
        => new(
            result.TenantId,
            result.RazaoSocial,
            result.LogoUrl,
            result.CorPrimaria,
            result.CorSecundaria,
            result.CorAccent);
}
