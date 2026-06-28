namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.SeedTenant;

public sealed record SeedTenantResponse(
    Guid TenantId,
    Guid? AdminUserId,
    string? SenhaInicial,
    bool EhNovo);
