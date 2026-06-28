using Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CompensarHoras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.CompensarHoras;

public static class CompensarHorasMap
{
    public static CompensarHorasCommand ToCommand(this CompensarHorasRequest r)
        => new(r.FuncionarioId, r.Data, r.Minutos, r.Motivo);

    public static CompensarHorasResponse ToResponse(this CompensarHorasCommandResult r)
        => new(r.MovimentoId, r.MinutosCompensados);
}
