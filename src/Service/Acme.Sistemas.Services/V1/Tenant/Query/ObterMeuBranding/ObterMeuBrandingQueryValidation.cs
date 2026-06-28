using FluentValidation;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterMeuBranding;

// Sem parâmetros para validar — query usa apenas ITenantContext.
public sealed class ObterMeuBrandingQueryValidation : AbstractValidator<ObterMeuBrandingQuery>
{
    public ObterMeuBrandingQueryValidation() { }
}
