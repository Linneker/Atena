using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Query.ListarAjustesPendentes;

public sealed class ListarAjustesPendentesQueryHandler
    : IRequestHandler<ListarAjustesPendentesQuery, ResponseDefault<ListarAjustesPendentesQueryResult>>
{
    private readonly IAjustePontoRepository _repo;

    public ListarAjustesPendentesQueryHandler(IAjustePontoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarAjustesPendentesQueryResult>> Handle(
        ListarAjustesPendentesQuery request, CancellationToken cancellationToken)
    {
        var ajustes = await _repo.ListarPendentesAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountPendentesAsync(cancellationToken);

        var items = ajustes.Select(a => new ListarAjustesPendentesQueryItem(
            a.Id, a.FuncionarioId, a.MarcacaoOriginalId, a.TipoAjuste,
            a.DataHoraProposta, a.Motivo, a.CreatedAt)).ToList();

        return ResponseDefault<ListarAjustesPendentesQueryResult>.Ok(
            new ListarAjustesPendentesQueryResult(items, total));
    }
}
