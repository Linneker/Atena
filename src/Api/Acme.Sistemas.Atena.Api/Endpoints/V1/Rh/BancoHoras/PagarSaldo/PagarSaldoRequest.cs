namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.PagarSaldo;

public sealed record PagarSaldoRequest(Guid FuncionarioId, string Competencia, int Minutos);
