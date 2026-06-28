using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ListarFaturamentos;

public sealed class ListarFaturamentosQueryHandler
    : IRequestHandler<ListarFaturamentosQuery, ResponseDefault<ListarFaturamentosQueryResult>>
{
    private readonly IFaturamentoRepository _repo;

    public ListarFaturamentosQueryHandler(IFaturamentoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarFaturamentosQueryResult>> Handle(
        ListarFaturamentosQuery request,
        CancellationToken cancellationToken)
    {
        var faturamentos = await _repo.ListByFiltroAsync(
            request.Inicio, request.Fim, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(
            request.Inicio, request.Fim, cancellationToken);

        var items = faturamentos.Select(f => new ListarFaturamentosQueryItem(
            f.Id, f.Numero, f.PedidoVendaId,
            f.DataFaturamento, f.Tipo, f.ValorTotal,
            f.NFeId, f.ContaReceberId)).ToList();

        return ResponseDefault<ListarFaturamentosQueryResult>.Ok(
            new ListarFaturamentosQueryResult(items, total, request.Skip, request.Take));
    }
}
