using Acme.Sistemas.Services.V1.Relatorios.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Vendas;

public static class RelatorioVendasMap
{
    public static RelatorioVendasQuery ToQuery(this RelatorioVendasRequest request)
        => new(request.Inicio, request.Fim, request.Agrupamento);

    public static RelatorioVendasResponse ToResponse(this RelatorioVendasResult result)
        => new(
            result.Inicio,
            result.Fim,
            result.Agrupamento,
            result.TotalGeral,
            result.Linhas.Select(l => l.ToResponseLinha()).ToArray());

    private static RelatorioVendasResponseLinha ToResponseLinha(this RelatorioVendasLinha linha)
        => new(linha.Id, linha.Quantidade, linha.Total, linha.Faturamentos);
}
