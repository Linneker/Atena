using Acme.Sistemas.Services.V1.Rh.Jornada.Command.CriarJornada;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.CriarJornada;

public static class CriarJornadaMap
{
    public static CriarJornadaCommand ToCommand(this CriarJornadaRequest r)
        => new(r.Nome, r.Tipo, r.CargaSemanalHoras, r.CargaDiariaHoras,
               r.JanelasJson, r.PermiteMarcarIntervalo, r.ToleranciaMinutos);

    public static CriarJornadaResponse ToResponse(this CriarJornadaCommandResult r)
        => new(r.Id, r.Nome);
}
