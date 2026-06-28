using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.RegistrarDispositivo;

public sealed record RegistrarDispositivoRequest(
    string DeviceId,
    PlataformaMobile Plataforma,
    string Modelo,
    string OsVersion,
    string AppVersion,
    string? PushToken,
    string? ChavePublicaLocal);
