using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

/// <summary>
/// Behavior específico do CriarTenantCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarTenantCommandBehavior
    : IPipelineBehavior<CriarTenantCommand, ResponseDefault<CriarTenantCommandResult>>
{
    public Task<ResponseDefault<CriarTenantCommandResult>> Handle(
        CriarTenantCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarTenantCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
