using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

public sealed record ObterContaPagarQuery(Guid Id) : IRequest<ResponseDefault<ObterContaPagarQueryResult>>;

