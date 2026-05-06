using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;

public sealed class ListarContasReceberQueryHandler
    : IRequestHandler<ListarContasReceberQuery, ResponseDefault<ListarContasReceberQueryResult>>
{
    private readonly IContaReceberRepository _repo;

    public ListarContasReceberQueryHandler(IContaReceberRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarContasReceberQueryResult>> Handle(ListarContasReceberQuery request, CancellationToken cancellationToken)
    {
        var contas = await _repo.ListByFiltroAsync(
            request.Status, request.VencimentoInicio, request.VencimentoFim,
            request.ClienteId, request.DiasAtrasoMinimo,
            request.Skip, request.Take, cancellationToken);

        var total = await _repo.CountByFiltroAsync(
            request.Status, request.VencimentoInicio, request.VencimentoFim,
            request.ClienteId, request.DiasAtrasoMinimo, cancellationToken);

        var hoje = DateTime.UtcNow.Date;
        var items = contas.Select(c => new ListarContasReceberQueryItem(
            c.Id, c.Descricao, c.ClienteId,
            c.ValorOriginal, c.ValorRecebido, c.Saldo,
            c.DataVencimento, c.Status, c.DiasAtrasoEm(hoje))).ToList();

        return ResponseDefault<ListarContasReceberQueryResult>.Ok(
            new ListarContasReceberQueryResult(items, total, request.Skip, request.Take));
    }
}
