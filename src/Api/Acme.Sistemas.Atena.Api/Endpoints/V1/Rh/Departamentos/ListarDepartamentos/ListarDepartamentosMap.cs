using Acme.Sistemas.Services.V1.Rh.Departamento.Query.ListarDepartamentos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.ListarDepartamentos;

public static class ListarDepartamentosMap
{
    public static ListarDepartamentosQuery ToQuery(this ListarDepartamentosRequest r)
        => new(r.Skip, r.Take);

    public static ListarDepartamentosResponse ToResponse(this ListarDepartamentosQueryResult r)
        => new(
            r.Items.Select(i => new ListarDepartamentosResponseItem(
                i.Id, i.Codigo, i.Nome, i.CentroDeCustoId, i.Ativo)).ToList(),
            r.Total);
}
