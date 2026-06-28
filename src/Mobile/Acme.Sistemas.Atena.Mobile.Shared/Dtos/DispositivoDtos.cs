namespace Acme.Sistemas.Atena.Mobile.Shared.Dtos;

public enum PlataformaMobileDto { Android, iOS, Windows, MacOS }

public sealed record RegistrarDispositivoRequest(
    string DeviceId,
    PlataformaMobileDto Plataforma,
    string Modelo,
    string OsVersion,
    string AppVersion,
    string? PushToken,
    string? ChavePublicaLocal);

public sealed record RegistrarDispositivoResponse(string DispositivoId, bool JaExistia);

public sealed record DispositivoDto(
    string Id,
    string DeviceId,
    PlataformaMobileDto Plataforma,
    string Modelo,
    string OsVersion,
    string AppVersion,
    bool Ativo,
    DateTime RegistradoEm,
    DateTime? UltimoAcesso);
