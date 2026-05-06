using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;

public sealed class ConsultarSaldoQueryHandler
    : IRequestHandler<ConsultarSaldoQuery, ResponseDefault<ConsultarSaldoQueryResult>>
{
    private readonly IEstoqueProdutoRepository _repo;

    public ConsultarSaldoQueryHandler(IEstoqueProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ConsultarSaldoQueryResult>> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
    {
        var saldos = await _repo.ListByProdutoAsync(request.ProdutoId, cancellationToken);

        var filtered = request.EstoqueId.HasValue
            ? saldos.Where(s => s.EstoqueId == request.EstoqueId.Value).ToList()
            : saldos.ToList();

        var porEstoque = filtered
            .Select(s => new SaldoPorEstoque(s.EstoqueId, s.SaldoTotal, s.SaldoReservado, s.SaldoDisponivel))
            .ToList();

        var total = filtered.Sum(s => s.SaldoTotal);
        var reservado = filtered.Sum(s => s.SaldoReservado);

        return ResponseDefault<ConsultarSaldoQueryResult>.Ok(new ConsultarSaldoQueryResult(
            request.ProdutoId,
            total,
            reservado,
            total - reservado,
            porEstoque));
    }
}
