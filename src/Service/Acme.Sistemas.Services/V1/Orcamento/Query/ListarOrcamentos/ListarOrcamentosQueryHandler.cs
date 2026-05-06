using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Orcamento.Query.ListarOrcamentos;

public sealed class ListarOrcamentosQueryHandler
    : IRequestHandler<ListarOrcamentosQuery, ResponseDefault<ListarOrcamentosQueryResult>>
{
    private readonly IOrcamentoRepository _repo;
    public ListarOrcamentosQueryHandler(IOrcamentoRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarOrcamentosQueryResult>> Handle(ListarOrcamentosQuery request, CancellationToken cancellationToken)
    {
        var orcs = await _repo.ListByFiltroAsync(request.Status, request.ClienteId, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(request.Status, request.ClienteId, cancellationToken);
        var items = orcs.Select(o => new ListarOrcamentosQueryItem(
            o.Id, o.Numero, o.ClienteId, o.VendedorId,
            o.DataEmissao, o.DataValidade, o.ValorTotal, o.Status)).ToList();
        return ResponseDefault<ListarOrcamentosQueryResult>.Ok(
            new ListarOrcamentosQueryResult(items, total));
    }
}
