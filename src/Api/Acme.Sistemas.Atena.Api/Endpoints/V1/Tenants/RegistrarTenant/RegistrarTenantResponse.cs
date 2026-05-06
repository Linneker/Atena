namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.RegistrarTenant;

public sealed record RegistrarTenantResponse(
    Guid Id,
    string RazaoSocial,
    string Cnpj,
    string Plano,
    Guid AdminUserId,
    string AdminEmail);
