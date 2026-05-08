using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarPermissoes;

/// <summary>
/// Behavior específico do ListarPermissoesQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarPermissoesQueryBehavior
    : IPipelineBehavior<ListarPermissoesQuery, ResponseDefault<ListarPermissoesQueryResult>>
{
    public Task<ResponseDefault<ListarPermissoesQueryResult>> Handle(
        ListarPermissoesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarPermissoesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
