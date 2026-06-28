using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CodigoServico.Query.ListarCodigosServico;

/// <summary>Behavior do ListarCodigosServicoQuery. No-op pass-through — convenção do blueprint.</summary>
public sealed class ListarCodigosServicoQueryBehavior
    : IPipelineBehavior<ListarCodigosServicoQuery, ResponseDefault<ListarCodigosServicoQueryResult>>
{
    public Task<ResponseDefault<ListarCodigosServicoQueryResult>> Handle(
        ListarCodigosServicoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarCodigosServicoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
