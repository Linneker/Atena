using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

public sealed record ObterSolicitacaoQuery(Guid Id) : IRequest<ResponseDefault<ObterSolicitacaoQueryResult>>;

