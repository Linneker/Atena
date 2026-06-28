namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BancoHoras.PagarSaldo;

public sealed record PagarSaldoResponse(
    Guid MovimentoId, int MinutosPagos, string Competencia, string PendenciaFolha);
