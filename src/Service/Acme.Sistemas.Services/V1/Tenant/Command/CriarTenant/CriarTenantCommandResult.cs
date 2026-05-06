namespace Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

public sealed record CriarTenantCommandResult(
    Guid Id,
    string RazaoSocial,
    string Cnpj,
    string Plano,
    Guid AdminUserId,
    string AdminEmail);
