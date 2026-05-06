using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Estoque.Services;

/// <summary>
/// Calcula CMV (Custo de Mercadoria Vendida) usando FIFO:
/// consome lotes mais antigos primeiro, atualiza quantidade restante
/// e devolve o custo unitário médio ponderado da saída.
/// </summary>
public sealed class FifoCustoCalculator
{
    private readonly IEntradaProdutoEstoqueRepository _entradas;

    public FifoCustoCalculator(IEntradaProdutoEstoqueRepository entradas)
    {
        _entradas = entradas;
    }

    public async Task<FifoResult> ConsumirAsync(
        Guid estoqueId, Guid produtoId, decimal quantidade,
        CancellationToken cancellationToken = default)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser positiva.", nameof(quantidade));

        var lotes = await _entradas.ListLotesAbertosFifoAsync(estoqueId, produtoId, cancellationToken);

        decimal restante = quantidade;
        decimal custoTotal = 0;
        decimal consumidoComCusto = 0;
        var consumidos = new List<FifoConsumo>();

        foreach (var lote in lotes)
        {
            if (restante <= 0) break;

            var consumir = Math.Min(restante, lote.QuantidadeRestante);
            if (consumir <= 0) continue;

            await _entradas.ConsumirLoteAsync(lote.Id, consumir, cancellationToken);

            if (lote.CustoUnitario.HasValue)
            {
                custoTotal += consumir * lote.CustoUnitario.Value;
                consumidoComCusto += consumir;
            }

            consumidos.Add(new FifoConsumo(lote.Id, consumir, lote.CustoUnitario));
            restante -= consumir;
        }

        decimal? cmvUnitario = consumidoComCusto > 0 ? custoTotal / consumidoComCusto : null;
        bool consumoCompleto = restante <= 0;

        return new FifoResult(consumidos, cmvUnitario, consumoCompleto, restante);
    }
}

public sealed record FifoConsumo(Guid LoteId, decimal Quantidade, decimal? CustoUnitario);

public sealed record FifoResult(
    IReadOnlyList<FifoConsumo> Consumos,
    decimal? CmvUnitarioMedio,
    bool ConsumoCompleto,
    decimal QuantidadeNaoCoberta);
