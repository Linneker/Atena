namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.CompensarHoras;

public sealed record CompensarHorasCommandResult(Guid MovimentoId, int MinutosCompensados);
