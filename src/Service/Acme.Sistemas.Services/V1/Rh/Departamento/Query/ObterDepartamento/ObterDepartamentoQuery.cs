using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ObterDepartamento;

public sealed record ObterDepartamentoQuery(Guid Id) : IRequest<ResponseDefault<ObterDepartamentoQueryResult>>;
