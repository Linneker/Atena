using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

/// <summary>
/// Behavior específico do ObterDividaQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterDividaQueryBehavior
    : IPipelineBehavior<ObterDividaQuery, ResponseDefault<ObterDividaQueryResult>>
{
    public Task<ResponseDefault<ObterDividaQueryResult>> Handle(
        ObterDividaQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterDividaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
