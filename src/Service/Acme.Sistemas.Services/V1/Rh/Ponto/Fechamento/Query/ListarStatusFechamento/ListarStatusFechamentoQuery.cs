using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Query.ListarStatusFechamento;

public sealed record ListarStatusFechamentoQuery(string Competencia)
    : IRequest<ResponseDefault<ListarStatusFechamentoQueryResult>>;
