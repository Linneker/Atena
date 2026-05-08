using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarRoles;

/// <summary>
/// Behavior específico do ListarRolesQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarRolesQueryBehavior
    : IPipelineBehavior<ListarRolesQuery, ResponseDefault<ListarRolesQueryResult>>
{
    public Task<ResponseDefault<ListarRolesQueryResult>> Handle(
        ListarRolesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarRolesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
