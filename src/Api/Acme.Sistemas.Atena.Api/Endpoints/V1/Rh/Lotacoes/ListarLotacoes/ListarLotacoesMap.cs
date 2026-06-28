using Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ListarLotacoes;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Lotacoes.ListarLotacoes;

public static class ListarLotacoesMap
{
    public static ListarLotacoesQuery ToQuery(this ListarLotacoesRequest r)
        => new(r.Skip, r.Take);

    public static ListarLotacoesResponse ToResponse(this ListarLotacoesQueryResult r)
        => new(
            r.Items.Select(i => new ListarLotacoesResponseItem(
                i.Id, i.Nome, i.EmpresaId, i.Cnpj, i.Ativo)).ToList(),
            r.Total);
}
