using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ListarFaturamentos;

public sealed record ListarFaturamentosQuery(
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarFaturamentosQueryResult>>;
