using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ListarContasPagar;

public sealed class ListarContasPagarQueryHandler
    : IRequestHandler<ListarContasPagarQuery, ResponseDefault<ListarContasPagarQueryResult>>
{
    private readonly IContaPagarRepository _repo;

    public ListarContasPagarQueryHandler(IContaPagarRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarContasPagarQueryResult>> Handle(ListarContasPagarQuery request, CancellationToken cancellationToken)
    {
        var contas = await _repo.ListByFiltroAsync(
            request.Status, request.VencimentoInicio, request.VencimentoFim,
            request.FornecedorId, request.VencendoEmAteSeteDias,
            request.Skip, request.Take, cancellationToken);

        var total = await _repo.CountByFiltroAsync(
            request.Status, request.VencimentoInicio, request.VencimentoFim,
            request.FornecedorId, request.VencendoEmAteSeteDias, cancellationToken);

        var hoje = DateTime.UtcNow.Date;
        var items = contas.Select(c => new ListarContasPagarQueryItem(
            c.Id, c.Descricao, c.FornecedorId,
            c.ValorOriginal, c.ValorPago, c.Saldo,
            c.DataVencimento, c.Status,
            c.VencidaEm(hoje),
            (c.DataVencimento.Date - hoje).Days)).ToList();

        return ResponseDefault<ListarContasPagarQueryResult>.Ok(
            new ListarContasPagarQueryResult(items, total, request.Skip, request.Take));
    }
}
