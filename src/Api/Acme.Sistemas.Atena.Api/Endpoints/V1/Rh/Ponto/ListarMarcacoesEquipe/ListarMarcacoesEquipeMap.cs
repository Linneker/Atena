using Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesEquipe;

public static class ListarMarcacoesEquipeMap
{
    public static ListarMarcacoesPorPeriodoQuery ToQuery(this ListarMarcacoesEquipeRequest r)
        => new(r.FuncionarioId, r.DataInicio, r.DataFim);

    public static ListarMarcacoesEquipeResponse ToResponse(this ListarMarcacoesPorPeriodoQueryResult r)
        => new(
            r.Items.Select(i => new ListarMarcacoesEquipeResponseItem(
                i.Id, i.DataHora, i.Tipo, i.Origem, i.Status)).ToList(),
            r.Total);
}
