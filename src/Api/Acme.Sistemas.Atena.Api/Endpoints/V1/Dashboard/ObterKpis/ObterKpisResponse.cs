namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dashboard.ObterKpis;

public sealed record ObterKpisResponse(
    DateTime Inicio,
    DateTime Fim,
    decimal Receita,
    decimal Despesa,
    decimal Resultado,
    int VendasAbertas,
    int ContasReceberVencendoEmAteSeteDias,
    int ContasPagarVencendoEmAteSeteDias,
    int ProdutosEmEstoqueCritico);
