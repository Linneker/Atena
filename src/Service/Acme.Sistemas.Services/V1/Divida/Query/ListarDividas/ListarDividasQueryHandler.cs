using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Divida.Query.ListarDividas;

public sealed class ListarDividasQueryHandler
    : IRequestHandler<ListarDividasQuery, ResponseDefault<ListarDividasQueryResult>>
{
    private readonly IDividaRepository _repo;

    public ListarDividasQueryHandler(IDividaRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarDividasQueryResult>> Handle(ListarDividasQuery request, CancellationToken cancellationToken)
    {
        var dividas = await _repo.ListByFiltroAsync(request.Status, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(request.Status, cancellationToken);

        var items = dividas.Select(d => new ListarDividasQueryItem(
            d.Id, d.Credor, d.ValorOriginal, d.ValorPago, d.Saldo,
            d.DataInicio, d.DataFim, d.NumeroParcelas, d.Status)).ToList();

        return ResponseDefault<ListarDividasQueryResult>.Ok(
            new ListarDividasQueryResult(items, total));
    }
}
