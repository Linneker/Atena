namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.PagarSaldo;

public sealed record PagarSaldoCommandResult(
    Guid MovimentoId,
    int MinutosPagos,
    string Competencia,
    string PendenciaFolha);
