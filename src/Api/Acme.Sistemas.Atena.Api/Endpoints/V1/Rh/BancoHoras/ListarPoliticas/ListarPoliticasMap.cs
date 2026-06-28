using Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarPoliticas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ListarPoliticas;

public static class ListarPoliticasMap
{
    public static ListarPoliticasQuery ToQuery(this ListarPoliticasRequest r) => new(r.Skip, r.Take);

    public static ListarPoliticasResponse ToResponse(this ListarPoliticasQueryResult r)
        => new(
            r.Items.Select(i => new ListarPoliticasResponseItem(
                i.Id, i.Nome, i.LimiteHorasAcumular,
                i.PrazoCompensacaoDias, i.PermitePagarExcedente, i.Ativo)).ToList(),
            r.Total);
}
