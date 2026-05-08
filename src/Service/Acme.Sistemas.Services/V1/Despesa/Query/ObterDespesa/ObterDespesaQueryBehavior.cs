using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ObterDespesa;

/// <summary>
/// Behavior específico do ObterDespesaQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterDespesaQueryBehavior
    : IPipelineBehavior<ObterDespesaQuery, ResponseDefault<ObterDespesaQueryResult>>
{
    public Task<ResponseDefault<ObterDespesaQueryResult>> Handle(
        ObterDespesaQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterDespesaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
