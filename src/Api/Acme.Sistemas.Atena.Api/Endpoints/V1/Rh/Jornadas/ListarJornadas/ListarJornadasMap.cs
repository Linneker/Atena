using Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.ListarJornadas;

public static class ListarJornadasMap
{
    public static ListarJornadasQuery ToQuery(this ListarJornadasRequest request)
        => new(request.Skip, request.Take);

    public static ListarJornadasResponse ToResponse(this ListarJornadasQueryResult result)
        => new(
            result.Items.Select(i => new ListarJornadasResponseItem(
                i.Id, i.Nome, i.Tipo, i.CargaSemanalHoras, i.CargaDiariaHoras,
                i.ToleranciaMinutos, i.Ativo)).ToList(),
            result.Total);
}
