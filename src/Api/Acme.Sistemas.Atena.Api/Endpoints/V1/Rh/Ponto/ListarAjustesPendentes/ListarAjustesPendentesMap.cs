using Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Query.ListarAjustesPendentes;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarAjustesPendentes;

public static class ListarAjustesPendentesMap
{
    public static ListarAjustesPendentesQuery ToQuery(this ListarAjustesPendentesRequest r)
        => new(r.Skip, r.Take);

    public static ListarAjustesPendentesResponse ToResponse(this ListarAjustesPendentesQueryResult r)
        => new(
            r.Items.Select(i => new ListarAjustesPendentesResponseItem(
                i.Id, i.FuncionarioId, i.MarcacaoOriginalId, i.TipoAjuste,
                i.DataHoraProposta, i.Motivo, i.SolicitadoEm)).ToList(),
            r.Total);
}
