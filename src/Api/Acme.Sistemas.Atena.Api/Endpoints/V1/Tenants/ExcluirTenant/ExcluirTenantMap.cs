using Acme.Sistemas.Services.V1.Tenant.Command.ExcluirTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.ExcluirTenant;

public static class ExcluirTenantMap
{
    public static ExcluirTenantCommand ToCommand(this ExcluirTenantRequest request)
        => new(request.Id);
}
