using Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarMarcacoesProprio;

public static class ListarMarcacoesProprioMap
{
    public static ListarMarcacoesPorPeriodoQuery ToQuery(this ListarMarcacoesProprioRequest r, Guid funcionarioId)
        => new(funcionarioId, r.DataInicio, r.DataFim);

    public static ListarMarcacoesProprioResponse ToResponse(this ListarMarcacoesPorPeriodoQueryResult r)
        => new(
            r.Items.Select(i => new ListarMarcacoesProprioResponseItem(
                i.Id, i.DataHora, i.Tipo, i.Origem, i.Status, i.HashIntegridade)).ToList(),
            r.Total);
}
