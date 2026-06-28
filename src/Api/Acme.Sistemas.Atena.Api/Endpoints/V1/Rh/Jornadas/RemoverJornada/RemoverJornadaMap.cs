using Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.RemoverJornada;

public static class RemoverJornadaMap
{
    public static RemoverJornadaCommand ToCommand(this RemoverJornadaRequest r)
        => new(r.Id);

    public static RemoverJornadaResponse ToResponse(this RemoverJornadaCommandResult r)
        => new(r.Id);
}
