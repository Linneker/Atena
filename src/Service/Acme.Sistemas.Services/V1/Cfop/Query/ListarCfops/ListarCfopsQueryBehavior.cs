using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cfop.Query.ListarCfops;

/// <summary>Behavior do ListarCfopsQuery. No-op pass-through — convenção do blueprint.</summary>
public sealed class ListarCfopsQueryBehavior
    : IPipelineBehavior<ListarCfopsQuery, ResponseDefault<ListarCfopsQueryResult>>
{
    public Task<ResponseDefault<ListarCfopsQueryResult>> Handle(
        ListarCfopsQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarCfopsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
