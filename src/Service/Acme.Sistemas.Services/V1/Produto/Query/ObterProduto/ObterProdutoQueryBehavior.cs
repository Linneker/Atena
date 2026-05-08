using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

/// <summary>
/// Behavior específico do ObterProdutoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterProdutoQueryBehavior
    : IPipelineBehavior<ObterProdutoQuery, ResponseDefault<ObterProdutoQueryResult>>
{
    public Task<ResponseDefault<ObterProdutoQueryResult>> Handle(
        ObterProdutoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterProdutoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
