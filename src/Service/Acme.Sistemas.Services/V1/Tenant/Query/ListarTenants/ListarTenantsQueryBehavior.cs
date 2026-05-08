using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

/// <summary>
/// Behavior específico do ListarTenantsQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarTenantsQueryBehavior
    : IPipelineBehavior<ListarTenantsQuery, ResponseDefault<ListarTenantsQueryResult>>
{
    public Task<ResponseDefault<ListarTenantsQueryResult>> Handle(
        ListarTenantsQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarTenantsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
