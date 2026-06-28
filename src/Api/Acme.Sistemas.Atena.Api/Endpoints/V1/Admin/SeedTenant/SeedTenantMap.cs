using Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.SeedTenant;

public static class SeedTenantMap
{
    public static SeedTenantCommand ToCommand(this SeedTenantRequest r)
        => new(r.Cnpj, r.RazaoSocial, r.AdminEmail);

    public static SeedTenantResponse ToResponse(this SeedTenantCommandResult result)
        => new(result.TenantId, result.AdminUserId, result.SenhaInicial, result.EhNovo);
}
