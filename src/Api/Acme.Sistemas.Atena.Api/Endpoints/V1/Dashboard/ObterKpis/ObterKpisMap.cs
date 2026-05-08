using Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dashboard.ObterKpis;

public static class ObterKpisMap
{
    public static ObterKpisQuery ToQuery(this ObterKpisRequest request)
        => new(request.Inicio, request.Fim);

    public static ObterKpisResponse ToResponse(this ObterKpisQueryResult result)
        => new(result.Inicio, result.Fim, result.Receita, result.Despesa, result.Resultado,
            result.VendasAbertas, result.ContasReceberVencendoEmAteSeteDias,
            result.ContasPagarVencendoEmAteSeteDias, result.ProdutosEmEstoqueCritico);
}
