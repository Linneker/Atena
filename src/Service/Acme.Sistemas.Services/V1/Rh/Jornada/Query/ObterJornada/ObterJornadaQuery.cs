using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;

public sealed record ObterJornadaQuery(Guid Id) : IRequest<ResponseDefault<ObterJornadaQueryResult>>;
