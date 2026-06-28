namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.SeedTenant;

public sealed record SeedTenantRequest(string Cnpj, string RazaoSocial, string AdminEmail);
