using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

public sealed record ObterReceitaQuery(Guid Id) : IRequest<ResponseDefault<ObterReceitaQueryResult>>;
