using Acme.Sistemas.Services.V1.Rh.Cargo.Command.AlterarCargo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.AlterarCargo;

public static class AlterarCargoMap
{
    public static AlterarCargoCommand ToCommand(this AlterarCargoRequest r)
        => new(r.Id, r.Codigo, r.Descricao, r.CodigoCbo, r.SalarioBaseSugerido, r.Ativo);

    public static AlterarCargoResponse ToResponse(this AlterarCargoCommandResult r)
        => new(r.Id);
}
