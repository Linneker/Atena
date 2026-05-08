namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dashboard.EvolucaoFinanceira;

public sealed record EvolucaoMesItemResponse(int Ano, int Mes, decimal Receitas, decimal Despesas, decimal Resultado);

public sealed record EvolucaoFinanceiraResponse(
    int Meses,
    IReadOnlyList<EvolucaoMesItemResponse> Pontos,
    decimal TotalReceitas,
    decimal TotalDespesas);
