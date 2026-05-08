using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Query.ListarTiposValorProduto;

/// <summary>
/// Behavior específico do ListarTiposValorProdutoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarTiposValorProdutoQueryBehavior
    : IPipelineBehavior<ListarTiposValorProdutoQuery, ResponseDefault<ListarTiposValorProdutoQueryResult>>
{
    public Task<ResponseDefault<ListarTiposValorProdutoQueryResult>> Handle(
        ListarTiposValorProdutoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarTiposValorProdutoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
