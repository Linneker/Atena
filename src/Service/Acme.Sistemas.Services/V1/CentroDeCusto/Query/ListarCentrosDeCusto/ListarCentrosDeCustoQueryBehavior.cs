using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Query.ListarCentrosDeCusto;

/// <summary>
/// Behavior específico do ListarCentrosDeCustoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarCentrosDeCustoQueryBehavior
    : IPipelineBehavior<ListarCentrosDeCustoQuery, ResponseDefault<ListarCentrosDeCustoQueryResult>>
{
    public Task<ResponseDefault<ListarCentrosDeCustoQueryResult>> Handle(
        ListarCentrosDeCustoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarCentrosDeCustoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
