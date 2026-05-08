using Acme.Sistemas.Domain.Reports;
using Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Dre.GerarDre;

public static class GerarDreMap
{
    public static GerarDREQuery ToQuery(this GerarDreRequest request)
        => new(request.Inicio, request.Fim);

    public static GerarDreResponse ToResponse(this DREResult result)
        => new(result.Inicio, result.Fim,
            result.Receitas.Select(l => l.ToResponseLinha()).ToArray(),
            result.Despesas.Select(l => l.ToResponseLinha()).ToArray(),
            result.TotalReceitas, result.TotalDespesas, result.ResultadoLiquido);

    private static DreLinhaResponse ToResponseLinha(this DRELinha linha)
        => new(linha.PlanoId, linha.Codigo, linha.Nome, linha.Nivel,
            linha.Valor, linha.Total,
            linha.Filhos.Select(f => f.ToResponseLinha()).ToList());
}
