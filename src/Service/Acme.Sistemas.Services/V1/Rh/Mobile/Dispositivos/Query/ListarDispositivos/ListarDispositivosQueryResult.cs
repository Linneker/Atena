using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Query.ListarDispositivos;

public sealed record ListarDispositivosQueryItem(
    Guid Id,
    Guid UsuarioId,
    Guid? FuncionarioId,
    string DeviceId,
    PlataformaMobile Plataforma,
    string? Modelo,
    string? OsVersion,
    string? AppVersion,
    bool Ativo,
    DateTime RegistradoEm,
    DateTime? UltimoAcesso);

public sealed record ListarDispositivosQueryResult(
    IReadOnlyList<ListarDispositivosQueryItem> Items, long Total);
