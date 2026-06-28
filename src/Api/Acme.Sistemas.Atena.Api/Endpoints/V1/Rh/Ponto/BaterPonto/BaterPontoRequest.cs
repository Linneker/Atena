using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterPonto;

public sealed record BaterPontoRequest(
    TipoMarcacao? Tipo,
    decimal? Latitude,
    decimal? Longitude,
    string? FotoUrl);
