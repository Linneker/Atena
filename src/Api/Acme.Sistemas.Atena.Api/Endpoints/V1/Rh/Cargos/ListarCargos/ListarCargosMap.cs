using Acme.Sistemas.Services.V1.Rh.Cargo.Query.ListarCargos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.ListarCargos;

public static class ListarCargosMap
{
    public static ListarCargosQuery ToQuery(this ListarCargosRequest r)
        => new(r.Skip, r.Take);

    public static ListarCargosResponse ToResponse(this ListarCargosQueryResult r)
        => new(
            r.Items.Select(i => new ListarCargosResponseItem(
                i.Id, i.Codigo, i.Descricao, i.CodigoCbo, i.SalarioBaseSugerido, i.Ativo)).ToList(),
            r.Total);
}
