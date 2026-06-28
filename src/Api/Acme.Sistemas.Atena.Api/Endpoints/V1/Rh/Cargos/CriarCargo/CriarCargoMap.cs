using Acme.Sistemas.Services.V1.Rh.Cargo.Command.CriarCargo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.CriarCargo;

public static class CriarCargoMap
{
    public static CriarCargoCommand ToCommand(this CriarCargoRequest r)
        => new(r.Codigo, r.Descricao, r.CodigoCbo, r.SalarioBaseSugerido);

    public static CriarCargoResponse ToResponse(this CriarCargoCommandResult r)
        => new(r.Id, r.Descricao);
}
