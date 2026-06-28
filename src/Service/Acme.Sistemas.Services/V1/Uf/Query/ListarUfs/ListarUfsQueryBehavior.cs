using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Uf.Query.ListarUfs;

/// <summary>
/// Behavior do ListarUfsQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior).
/// </summary>
public sealed class ListarUfsQueryBehavior
    : IPipelineBehavior<ListarUfsQuery, ResponseDefault<ListarUfsQueryResult>>
{
    public Task<ResponseDefault<ListarUfsQueryResult>> Handle(
        ListarUfsQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarUfsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
