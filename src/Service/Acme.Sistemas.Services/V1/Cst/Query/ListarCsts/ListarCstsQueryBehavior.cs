using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cst.Query.ListarCsts;

/// <summary>Behavior do ListarCstsQuery. No-op pass-through — convenção do blueprint.</summary>
public sealed class ListarCstsQueryBehavior
    : IPipelineBehavior<ListarCstsQuery, ResponseDefault<ListarCstsQueryResult>>
{
    public Task<ResponseDefault<ListarCstsQueryResult>> Handle(
        ListarCstsQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarCstsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
