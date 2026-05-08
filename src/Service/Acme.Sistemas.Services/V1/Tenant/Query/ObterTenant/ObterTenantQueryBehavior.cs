using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterTenant;

/// <summary>
/// Behavior específico do ObterTenantQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterTenantQueryBehavior
    : IPipelineBehavior<ObterTenantQuery, ResponseDefault<ObterTenantQueryResult>>
{
    public Task<ResponseDefault<ObterTenantQueryResult>> Handle(
        ObterTenantQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterTenantQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
