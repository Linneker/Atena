using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Query.ListarDispositivos;

public sealed class ListarDispositivosQueryHandler
    : IRequestHandler<ListarDispositivosQuery, ResponseDefault<ListarDispositivosQueryResult>>
{
    private readonly IDispositivoMobileRepository _repo;

    public ListarDispositivosQueryHandler(IDispositivoMobileRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarDispositivosQueryResult>> Handle(
        ListarDispositivosQuery request, CancellationToken cancellationToken)
    {
        var disp = await _repo.ListAllTenantAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountTenantAsync(cancellationToken);

        var items = disp.Select(d => new ListarDispositivosQueryItem(
            d.Id, d.UsuarioId, d.FuncionarioId, d.DeviceId, d.Plataforma,
            d.Modelo, d.OsVersion, d.AppVersion, d.Ativo, d.RegistradoEm, d.UltimoAcesso)).ToList();

        return ResponseDefault<ListarDispositivosQueryResult>.Ok(
            new ListarDispositivosQueryResult(items, total));
    }
}
