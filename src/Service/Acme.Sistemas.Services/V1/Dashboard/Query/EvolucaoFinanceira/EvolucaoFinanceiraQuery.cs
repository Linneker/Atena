using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.EvolucaoFinanceira;

public sealed record EvolucaoFinanceiraQuery(int Meses = 12)
    : IRequest<ResponseDefault<EvolucaoFinanceiraQueryResult>>;

public sealed record EvolucaoMesItem(int Ano, int Mes, decimal Receitas, decimal Despesas, decimal Resultado);

public sealed record EvolucaoFinanceiraQueryResult(
    int Meses,
    IReadOnlyList<EvolucaoMesItem> Pontos,
    decimal TotalReceitas,
    decimal TotalDespesas);
