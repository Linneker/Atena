using Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Query.ListarStatusFechamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarStatusFechamento;

public static class ListarStatusFechamentoMap
{
    public static ListarStatusFechamentoQuery ToQuery(this ListarStatusFechamentoRequest r) => new(r.Competencia);

    public static ListarStatusFechamentoResponse ToResponse(this ListarStatusFechamentoQueryResult r)
        => new(
            r.Items.Select(i => new ListarStatusFechamentoResponseItem(
                i.FuncionarioId, i.Status, i.FechadoEm)).ToList(),
            r.Total);
}
