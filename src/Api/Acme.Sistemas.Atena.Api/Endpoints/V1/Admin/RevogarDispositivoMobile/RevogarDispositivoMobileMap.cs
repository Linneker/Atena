using Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RevogarDispositivo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.RevogarDispositivoMobile;

public static class RevogarDispositivoMobileMap
{
    public static RevogarDispositivoCommand ToCommand(this RevogarDispositivoMobileRequest r)
        => new(r.Id);

    public static RevogarDispositivoMobileResponse ToResponse(this RevogarDispositivoCommandResult r)
        => new(r.Id);
}
