using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterMobile;

// Placeholder — multipart é desserializado direto no Endpoint.
public sealed record BaterMobileRequest(
    TipoMarcacao? Tipo,
    decimal? Latitude,
    decimal? Longitude,
    string DeviceId,
    DateTime TimestampLocal,
    string HashBatida,
    string? ProvaBiometriaLocal);
