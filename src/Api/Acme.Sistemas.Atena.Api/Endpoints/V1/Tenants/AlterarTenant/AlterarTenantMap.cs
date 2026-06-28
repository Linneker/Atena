using Acme.Sistemas.Services.V1.Tenant.Command.AlterarTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.AlterarTenant;

public static class AlterarTenantMap
{
    public static AlterarTenantCommand ToCommand(this AlterarTenantRequest request, Guid id)
        => new(
            id,
            request.RazaoSocial,
            request.Plano,
            request.Status,
            request.LogoUrl,
            request.CorPrimaria,
            request.FusoHorario);

    public static AlterarTenantResponse ToResponse(this AlterarTenantCommandResult result)
        => new(result.Id);
}
