using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Orcamento.Query.ListarOrcamentos;

/// <summary>
/// Behavior específico do ListarOrcamentosQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarOrcamentosQueryBehavior
    : IPipelineBehavior<ListarOrcamentosQuery, ResponseDefault<ListarOrcamentosQueryResult>>
{
    public Task<ResponseDefault<ListarOrcamentosQueryResult>> Handle(
        ListarOrcamentosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarOrcamentosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
