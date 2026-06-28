namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ListarTenants;

public sealed record ListarTenantsRequest(int Skip = 0, int Take = 50);
