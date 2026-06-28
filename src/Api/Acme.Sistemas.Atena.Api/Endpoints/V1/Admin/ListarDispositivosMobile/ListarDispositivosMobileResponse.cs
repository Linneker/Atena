using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.ListarDispositivosMobile;

public sealed record DispositivoMobileItem(
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

public sealed record ListarDispositivosMobileResponse(
    IReadOnlyList<DispositivoMobileItem> Items, long Total);
