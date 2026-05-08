using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

/// <summary>
/// Behavior específico do ObterSolicitacaoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterSolicitacaoQueryBehavior
    : IPipelineBehavior<ObterSolicitacaoQuery, ResponseDefault<ObterSolicitacaoQueryResult>>
{
    public Task<ResponseDefault<ObterSolicitacaoQueryResult>> Handle(
        ObterSolicitacaoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterSolicitacaoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
