using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ObterBeneficioCatalogo;

public sealed record ObterBeneficioCatalogoQuery(Guid Id)
    : IRequest<ResponseDefault<ObterBeneficioCatalogoQueryResult>>;
