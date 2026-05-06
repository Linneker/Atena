namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterTenant;

public sealed record ObterTenantQueryResult(
    Guid Id,
    string RazaoSocial,
    string Cnpj,
    string Plano,
    int Status,
    string? LogoUrl,
    string? CorPrimaria,
    string FusoHorario,
    DateTime CreatedAt);
