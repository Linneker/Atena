using Acme.Sistemas.Services.V1.Rh.Cargo.Command.RemoverCargo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.RemoverCargo;

public static class RemoverCargoMap
{
    public static RemoverCargoCommand ToCommand(this RemoverCargoRequest r) => new(r.Id);
    public static RemoverCargoResponse ToResponse(this RemoverCargoCommandResult r) => new(r.Id);
}
