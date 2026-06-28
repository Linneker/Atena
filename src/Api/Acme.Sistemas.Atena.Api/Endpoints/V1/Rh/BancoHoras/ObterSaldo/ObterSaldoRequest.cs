namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.ObterSaldo;

public sealed record ObterSaldoRequest(Guid FuncionarioId, string Competencia);
