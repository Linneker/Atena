using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ObterFaturamento;

public sealed record ObterFaturamentoQuery(Guid Id) : IRequest<ResponseDefault<ObterFaturamentoQueryResult>>;
