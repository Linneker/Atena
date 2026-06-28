namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ListarTenants;

public sealed record ListarTenantsResponseItem(
    Guid Id,
    string RazaoSocial,
    string Cnpj,
    string Plano,
    int Status,
    DateTime CreatedAt);

public sealed record ListarTenantsResponse(
    IReadOnlyList<ListarTenantsResponseItem> Items,
    int Total);
