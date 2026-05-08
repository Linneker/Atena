using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoProduto.Query.ListarTiposProduto;

/// <summary>
/// Behavior específico do ListarTiposProdutoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarTiposProdutoQueryBehavior
    : IPipelineBehavior<ListarTiposProdutoQuery, ResponseDefault<ListarTiposProdutoQueryResult>>
{
    public Task<ResponseDefault<ListarTiposProdutoQueryResult>> Handle(
        ListarTiposProdutoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarTiposProdutoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
