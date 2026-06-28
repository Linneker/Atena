namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ObterSaldo;

public sealed record ObterSaldoQueryResult(
    Guid FuncionarioId,
    string Competencia,
    decimal HorasDevidas,
    decimal HorasRealizadas,
    int SaldoMinutos,
    Guid? PoliticaId);
