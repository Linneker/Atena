namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.AlterarTenant;

public sealed record AlterarTenantRequest(
    string RazaoSocial,
    string Plano,
    int Status,
    string? LogoUrl,
    string? CorPrimaria,
    string FusoHorario);
