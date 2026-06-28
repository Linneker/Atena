using Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.DesregistrarDispositivo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Mobile.DesregistrarDispositivo;

public static class DesregistrarDispositivoMap
{
    public static DesregistrarDispositivoCommand ToCommand(this DesregistrarDispositivoRequest r)
        => new(r.DeviceId);

    public static DesregistrarDispositivoResponse ToResponse(this DesregistrarDispositivoCommandResult r)
        => new(r.DispositivoId);
}
