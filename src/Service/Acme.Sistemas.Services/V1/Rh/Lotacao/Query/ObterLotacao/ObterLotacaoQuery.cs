using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ObterLotacao;

public sealed record ObterLotacaoQuery(Guid Id) : IRequest<ResponseDefault<ObterLotacaoQueryResult>>;
