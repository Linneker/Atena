using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

public sealed record CriarTenantCommand(
    string RazaoSocial,
    string Cnpj,
    string Plano,
    string? FusoHorario,
    string? CorPrimaria,
    string? LogoUrl) : IRequest<ResponseDefault<CriarTenantCommandResult>>;
