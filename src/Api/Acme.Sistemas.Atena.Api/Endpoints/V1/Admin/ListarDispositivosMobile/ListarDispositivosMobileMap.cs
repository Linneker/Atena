using Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Query.ListarDispositivos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.ListarDispositivosMobile;

public static class ListarDispositivosMobileMap
{
    public static ListarDispositivosQuery ToQuery(this ListarDispositivosMobileRequest r) => new(r.Skip, r.Take);

    public static ListarDispositivosMobileResponse ToResponse(this ListarDispositivosQueryResult r)
        => new(
            r.Items.Select(i => new DispositivoMobileItem(
                i.Id, i.UsuarioId, i.FuncionarioId, i.DeviceId, i.Plataforma,
                i.Modelo, i.OsVersion, i.AppVersion, i.Ativo, i.RegistradoEm, i.UltimoAcesso)).ToList(),
            r.Total);
}
