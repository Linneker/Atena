using Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Tenants.RegistrarTenant;

public static class RegistrarTenantMap
{
    public static CriarTenantCommand ToCommand(this RegistrarTenantRequest request) =>
        new(request.RazaoSocial, request.Cnpj, request.Plano,
            request.FusoHorario, request.CorPrimaria, request.LogoUrl);

    public static RegistrarTenantResponse ToResponse(this CriarTenantCommandResult result) =>
        new(result.Id, result.RazaoSocial, result.Cnpj, result.Plano);
}
