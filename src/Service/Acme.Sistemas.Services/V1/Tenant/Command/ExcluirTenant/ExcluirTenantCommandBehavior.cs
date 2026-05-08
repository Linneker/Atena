using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Command.ExcluirTenant;

/// <summary>
/// Behavior específico do ExcluirTenantCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirTenantCommandBehavior
    : IPipelineBehavior<ExcluirTenantCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirTenantCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
