using Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.ObterJornada;

public static class ObterJornadaMap
{
    public static ObterJornadaQuery ToQuery(this ObterJornadaRequest request)
        => new(request.Id);

    public static ObterJornadaResponse ToResponse(this ObterJornadaQueryResult result)
        => new(
            result.Id, result.Nome, result.Tipo, result.CargaSemanalHoras,
            result.CargaDiariaHoras, result.JanelasJson, result.PermiteMarcarIntervalo,
            result.ToleranciaMinutos, result.Ativo);
}
