namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ObterSaldo;

public sealed record ObterSaldoResponse(
    Guid FuncionarioId, string Competencia,
    decimal HorasDevidas, decimal HorasRealizadas, int SaldoMinutos, Guid? PoliticaId);
