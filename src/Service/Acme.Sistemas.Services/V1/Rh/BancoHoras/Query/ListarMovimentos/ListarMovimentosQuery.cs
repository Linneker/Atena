using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarMovimentos;

public sealed record ListarMovimentosQuery(Guid FuncionarioId, string Competencia)
    : IRequest<ResponseDefault<ListarMovimentosQueryResult>>;
