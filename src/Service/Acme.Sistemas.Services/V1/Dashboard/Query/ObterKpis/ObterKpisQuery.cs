using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;

public sealed record ObterKpisQuery(
    DateTime? Inicio = null,
    DateTime? Fim = null) : IRequest<ResponseDefault<ObterKpisQueryResult>>;

public sealed record ObterKpisQueryResult(
    DateTime Inicio,
    DateTime Fim,
    decimal Receita,
    decimal Despesa,
    decimal Resultado,
    int VendasAbertas,
    int ContasReceberVencendoEmAteSeteDias,
    int ContasPagarVencendoEmAteSeteDias,
    int ProdutosEmEstoqueCritico);
