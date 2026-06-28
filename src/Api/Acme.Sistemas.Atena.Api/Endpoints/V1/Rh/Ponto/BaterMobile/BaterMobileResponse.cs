using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterMobile;

public sealed record BaterMobileResponse(
    Guid Id,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string HashIntegridade,
    string? FotoUrl);
