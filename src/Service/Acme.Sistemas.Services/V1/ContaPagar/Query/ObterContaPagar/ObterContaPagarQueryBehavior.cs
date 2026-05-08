using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

/// <summary>
/// Behavior específico do ObterContaPagarQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterContaPagarQueryBehavior
    : IPipelineBehavior<ObterContaPagarQuery, ResponseDefault<ObterContaPagarQueryResult>>
{
    public Task<ResponseDefault<ObterContaPagarQueryResult>> Handle(
        ObterContaPagarQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterContaPagarQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
