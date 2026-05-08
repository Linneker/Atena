using Acme.Sistemas.Services.V1.Dashboard.Query.EvolucaoFinanceira;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dashboard.EvolucaoFinanceira;

public static class EvolucaoFinanceiraMap
{
    public static EvolucaoFinanceiraQuery ToQuery(this EvolucaoFinanceiraRequest request)
        => new(request.Meses ?? 12);

    public static EvolucaoFinanceiraResponse ToResponse(this EvolucaoFinanceiraQueryResult result)
        => new(result.Meses,
            result.Pontos.Select(p => new EvolucaoMesItemResponse(p.Ano, p.Mes, p.Receitas, p.Despesas, p.Resultado)).ToArray(),
            result.TotalReceitas, result.TotalDespesas);
}
