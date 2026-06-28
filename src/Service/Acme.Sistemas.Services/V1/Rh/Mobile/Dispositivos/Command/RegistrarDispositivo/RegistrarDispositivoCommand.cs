using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RegistrarDispositivo;

public sealed record RegistrarDispositivoCommand(
    string DeviceId,
    PlataformaMobile Plataforma,
    string Modelo,
    string OsVersion,
    string AppVersion,
    string? PushToken,
    string? ChavePublicaLocal) : IRequest<ResponseDefault<RegistrarDispositivoCommandResult>>;
