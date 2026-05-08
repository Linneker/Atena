using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

/// <summary>
/// Behavior específico do ObterContaReceberQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterContaReceberQueryBehavior
    : IPipelineBehavior<ObterContaReceberQuery, ResponseDefault<ObterContaReceberQueryResult>>
{
    public Task<ResponseDefault<ObterContaReceberQueryResult>> Handle(
        ObterContaReceberQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterContaReceberQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
