using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ListarBeneficiosCatalogo;

public sealed class ListarBeneficiosCatalogoQueryBehavior
    : IPipelineBehavior<ListarBeneficiosCatalogoQuery, ResponseDefault<ListarBeneficiosCatalogoQueryResult>>
{
    public Task<ResponseDefault<ListarBeneficiosCatalogoQueryResult>> Handle(
        ListarBeneficiosCatalogoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarBeneficiosCatalogoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
