using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPontoMobile;

public sealed record BaterPontoMobileCommandResult(
    Guid Id,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string HashIntegridade,
    string? FotoUrl);
