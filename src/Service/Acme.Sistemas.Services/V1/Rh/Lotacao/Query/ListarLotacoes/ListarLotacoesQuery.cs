using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ListarLotacoes;

public sealed record ListarLotacoesQuery(
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarLotacoesQueryResult>>;
