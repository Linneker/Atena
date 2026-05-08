using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Query.ListarProdutos;

/// <summary>
/// Behavior específico do ListarProdutosQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarProdutosQueryBehavior
    : IPipelineBehavior<ListarProdutosQuery, ResponseDefault<ListarProdutosQueryResult>>
{
    public Task<ResponseDefault<ListarProdutosQueryResult>> Handle(
        ListarProdutosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarProdutosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
