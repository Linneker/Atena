using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;

public sealed class ListarContasReceberQueryHandler
    : IRequestHandler<ListarContasReceberQuery, ResponseDefault<ListarContasReceberQueryResult>>
{
    private readonly IContaReceberRepository _repo;
    private readonly IClienteRepository _clientes;

    public ListarContasReceberQueryHandler(IContaReceberRepository repo, IClienteRepository clientes)
    {
        _repo = repo;
        _clientes = clientes;
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

        var clienteIds = contas.Where(c => c.ClienteId.HasValue).Select(c => c.ClienteId!.Value);
        var nomesCliente = await _clientes.GetNomesByIdsAsync(clienteIds, cancellationToken);

        var hoje = DateTime.UtcNow.Date;
        var items = contas.Select(c => new ListarContasReceberQueryItem(
            c.Id, c.Descricao,
            c.ClienteId,
            c.ClienteId.HasValue && nomesCliente.TryGetValue(c.ClienteId.Value, out var nome) ? nome : null,
            c.ValorOriginal, c.ValorRecebido, c.Saldo,
            c.DataVencimento, c.Status, c.DiasAtrasoEm(hoje))).ToList();

        return ResponseDefault<ListarContasReceberQueryResult>.Ok(
            new ListarContasReceberQueryResult(items, total, request.Skip, request.Take));
    }
}
