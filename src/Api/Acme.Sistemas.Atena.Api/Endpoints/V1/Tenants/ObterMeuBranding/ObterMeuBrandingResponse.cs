namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ObterMeuBranding;

public sealed record ObterMeuBrandingResponse(
    Guid TenantId,
    string RazaoSocial,
    string? LogoUrl,
    string CorPrimaria,
    string CorSecundaria,
    string CorAccent);
