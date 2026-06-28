using Acme.Sistemas.Services.V1.Rh.Cbo.Query.ListarCbos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cbos.ListarCbos;

public static class ListarCbosMap
{
    public static ListarCbosQuery ToQuery(this ListarCbosRequest _) => new();

    public static ListarCbosResponse ToResponse(this ListarCbosQueryResult r)
        => new(
            r.Items.Select(i => new ListarCbosResponseItem(
                i.Codigo, i.Titulo, i.GrandeGrupo, i.Familia)).ToList(),
            r.Total);
}
