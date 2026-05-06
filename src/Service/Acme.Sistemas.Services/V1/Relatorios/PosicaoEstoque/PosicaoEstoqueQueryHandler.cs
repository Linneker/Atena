using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

public sealed class PosicaoEstoqueQueryHandler
    : IRequestHandler<PosicaoEstoqueQuery, ResponseDefault<PosicaoEstoqueQueryResult>>
{
    private readonly IPosicaoEstoqueRepository _repo;

    public PosicaoEstoqueQueryHandler(IPosicaoEstoqueRepository repo) => _repo = repo;

    public async Task<ResponseDefault<PosicaoEstoqueQueryResult>> Handle(PosicaoEstoqueQuery request, CancellationToken cancellationToken)
    {
        var raw = await _repo.ConsultarAsync(request.EstoqueId, cancellationToken);
        var linhas = raw.Select(l => new PosicaoEstoqueLinhaView(
            l.ProdutoId, l.CodigoProduto, l.NomeProduto,
            l.SaldoTotal, l.SaldoReservado, l.SaldoDisponivel,
            l.CustoMedio, l.ValorEstoque)).ToList();

        return ResponseDefault<PosicaoEstoqueQueryResult>.Ok(new PosicaoEstoqueQueryResult(
            request.EstoqueId,
            linhas.Count,
            linhas.Sum(l => l.ValorEstoque),
            linhas));
    }
}
