using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Aging;

public enum TipoAging { ContasPagar = 1, ContasReceber = 2 }

public sealed record AgingQuery(TipoAging Tipo) : IRequest<ResponseDefault<AgingQueryResult>>;

