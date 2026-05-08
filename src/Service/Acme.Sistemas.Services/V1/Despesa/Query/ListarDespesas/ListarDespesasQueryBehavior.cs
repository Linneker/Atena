using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

/// <summary>
/// Behavior específico do ListarDespesasQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarDespesasQueryBehavior
    : IPipelineBehavior<ListarDespesasQuery, ResponseDefault<ListarDespesasQueryResult>>
{
    public Task<ResponseDefault<ListarDespesasQueryResult>> Handle(
        ListarDespesasQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarDespesasQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
