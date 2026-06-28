namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterMeuBranding;

public sealed record ObterMeuBrandingQueryResult(
    Guid TenantId,
    string RazaoSocial,
    string? LogoUrl,
    string CorPrimaria,
    string CorSecundaria,
    string CorAccent);
