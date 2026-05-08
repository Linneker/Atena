using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Divida.Query.ListarDividas;

/// <summary>
/// Behavior específico do ListarDividasQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarDividasQueryBehavior
    : IPipelineBehavior<ListarDividasQuery, ResponseDefault<ListarDividasQueryResult>>
{
    public Task<ResponseDefault<ListarDividasQueryResult>> Handle(
        ListarDividasQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarDividasQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
