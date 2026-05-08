using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Command.AlterarTenant;

public sealed record AlterarTenantCommand(
    Guid Id,
    string RazaoSocial,
    string Plano,
    int Status,
    string? LogoUrl,
    string? CorPrimaria,
    string FusoHorario) : IRequest<ResponseDefault<AlterarTenantCommandResult>>;

