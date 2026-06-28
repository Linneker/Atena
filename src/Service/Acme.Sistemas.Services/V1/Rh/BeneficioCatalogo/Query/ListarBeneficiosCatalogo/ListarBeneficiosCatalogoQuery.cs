using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ListarBeneficiosCatalogo;

public sealed record ListarBeneficiosCatalogoQuery(
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarBeneficiosCatalogoQueryResult>>;
