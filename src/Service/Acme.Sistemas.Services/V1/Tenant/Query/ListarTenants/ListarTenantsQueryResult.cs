namespace Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

public sealed record ListarTenantsQueryItem(
    Guid Id, string RazaoSocial, string Cnpj, string Plano, int Status, DateTime CreatedAt);

public sealed record ListarTenantsQueryResult(IReadOnlyList<ListarTenantsQueryItem> Items, int Total);
