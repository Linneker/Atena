using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ListarContasPagar;

/// <summary>
/// Behavior específico do ListarContasPagarQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarContasPagarQueryBehavior
    : IPipelineBehavior<ListarContasPagarQuery, ResponseDefault<ListarContasPagarQueryResult>>
{
    public Task<ResponseDefault<ListarContasPagarQueryResult>> Handle(
        ListarContasPagarQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarContasPagarQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
