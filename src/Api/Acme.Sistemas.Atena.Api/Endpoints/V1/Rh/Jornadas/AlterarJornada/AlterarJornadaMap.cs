using Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.AlterarJornada;

public static class AlterarJornadaMap
{
    public static AlterarJornadaCommand ToCommand(this AlterarJornadaRequest r, Guid id)
        => new(id, r.Nome, r.Tipo, r.CargaSemanalHoras, r.CargaDiariaHoras,
               r.JanelasJson, r.PermiteMarcarIntervalo, r.ToleranciaMinutos, r.Ativo);

    public static AlterarJornadaResponse ToResponse(this AlterarJornadaCommandResult r)
        => new(r.Id);
}
