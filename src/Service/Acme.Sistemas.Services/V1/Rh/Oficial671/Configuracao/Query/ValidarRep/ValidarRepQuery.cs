using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ValidarRep;

public sealed record ValidarRepQuery(Guid EmpresaId) : IRequest<ResponseDefault<ValidarRepQueryResult>>;
