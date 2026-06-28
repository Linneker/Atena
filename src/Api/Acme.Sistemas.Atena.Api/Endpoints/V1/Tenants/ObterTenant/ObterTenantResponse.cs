namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ObterTenant;

public sealed record ObterTenantResponse(
    Guid Id,
    string RazaoSocial,
    string Cnpj,
    string Plano,
    int Status,
    string? LogoUrl,
    string? CorPrimaria,
    string FusoHorario,
    DateTime CreatedAt);
