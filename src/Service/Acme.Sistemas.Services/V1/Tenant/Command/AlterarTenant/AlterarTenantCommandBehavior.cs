using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Command.AlterarTenant;

/// <summary>
/// Behavior específico do AlterarTenantCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarTenantCommandBehavior
    : IPipelineBehavior<AlterarTenantCommand, ResponseDefault<AlterarTenantCommandResult>>
{
    public Task<ResponseDefault<AlterarTenantCommandResult>> Handle(
        AlterarTenantCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarTenantCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
