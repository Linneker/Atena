using Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.PagarSaldo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.PagarSaldo;

public static class PagarSaldoMap
{
    public static PagarSaldoCommand ToCommand(this PagarSaldoRequest r)
        => new(r.FuncionarioId, r.Competencia, r.Minutos);

    public static PagarSaldoResponse ToResponse(this PagarSaldoCommandResult r)
        => new(r.MovimentoId, r.MinutosPagos, r.Competencia, r.PendenciaFolha);
}
