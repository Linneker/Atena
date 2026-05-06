namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.RegistrarTenant;

public sealed record RegistrarTenantRequest(
    string RazaoSocial,
    string Cnpj,
    string Plano,
    string? FusoHorario,
    string? CorPrimaria,
    string? LogoUrl,
    string AdminNomeCompleto,
    string AdminEmail,
    string AdminSenha);
