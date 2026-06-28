namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.CompensarHoras;

public sealed record CompensarHorasRequest(Guid FuncionarioId, DateOnly Data, int Minutos, string Motivo);
