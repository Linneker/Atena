namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.CompensarHoras;

public sealed record CompensarHorasResponse(Guid MovimentoId, int MinutosCompensados);
