using Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RegistrarDispositivo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.RegistrarDispositivo;

public static class RegistrarDispositivoMap
{
    public static RegistrarDispositivoCommand ToCommand(this RegistrarDispositivoRequest r)
        => new(r.DeviceId, r.Plataforma, r.Modelo, r.OsVersion, r.AppVersion,
               r.PushToken, r.ChavePublicaLocal);

    public static RegistrarDispositivoResponse ToResponse(this RegistrarDispositivoCommandResult r)
        => new(r.DispositivoId, r.JaExistia);
}
