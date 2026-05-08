using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

public sealed record ObterContaReceberQuery(Guid Id) : IRequest<ResponseDefault<ObterContaReceberQueryResult>>;

