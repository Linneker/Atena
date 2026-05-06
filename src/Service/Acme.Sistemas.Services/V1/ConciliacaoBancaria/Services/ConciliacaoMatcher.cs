using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using ContaPagarEntity = Acme.Sistemas.Domain.Entities.Financeiro.ContaPagar;
using ContaReceberEntity = Acme.Sistemas.Domain.Entities.Financeiro.ContaReceber;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Services;

/// <summary>
/// Tentativa de match automático: para cada item de extrato,
/// busca uma ContaPagar (débito) ou ContaReceber (crédito) com mesmo valor
/// e data de vencimento dentro de uma janela tolerante (default ±3 dias).
/// </summary>
public sealed class ConciliacaoMatcher
{
    private readonly IContaPagarRepository _contasPagar;
    private readonly IContaReceberRepository _contasReceber;

    public ConciliacaoMatcher(
        IContaPagarRepository contasPagar,
        IContaReceberRepository contasReceber)
    {
        _contasPagar = contasPagar;
        _contasReceber = contasReceber;
    }

    public async Task<int> ConciliarAsync(
        IList<ItemExtrato> itens,
        int diasTolerancia = 3,
        CancellationToken cancellationToken = default)
    {
        var conciliados = 0;

        foreach (var item in itens)
        {
            if (item.Status != StatusItemExtrato.NaoConciliado) continue;

            if (item.Tipo == TipoMovimentoExtrato.Debito)
            {
                var match = await BuscarContaPagarAsync(item, diasTolerancia, cancellationToken);
                if (match is not null)
                {
                    item.ContaPagarId = match.Id;
                    item.Status = StatusItemExtrato.ConciliadoAutomaticamente;
                    conciliados++;
                }
            }
            else
            {
                var match = await BuscarContaReceberAsync(item, diasTolerancia, cancellationToken);
                if (match is not null)
                {
                    item.ContaReceberId = match.Id;
                    item.Status = StatusItemExtrato.ConciliadoAutomaticamente;
                    conciliados++;
                }
            }
        }

        return conciliados;
    }

    private async Task<ContaPagarEntity?> BuscarContaPagarAsync(ItemExtrato item, int dias, CancellationToken ct)
    {
        var inicio = item.DataMovimento.AddDays(-dias);
        var fim = item.DataMovimento.AddDays(dias);
        var candidatas = await _contasPagar.ListByFiltroAsync(
            StatusConta.Pendente, inicio, fim, fornecedorId: null,
            somenteVencendoEmAteSeteDias: false, skip: 0, take: 200, ct);
        return candidatas.FirstOrDefault(c => c.Saldo == item.Valor);
    }

    private async Task<ContaReceberEntity?> BuscarContaReceberAsync(ItemExtrato item, int dias, CancellationToken ct)
    {
        var inicio = item.DataMovimento.AddDays(-dias);
        var fim = item.DataMovimento.AddDays(dias);
        var candidatas = await _contasReceber.ListByFiltroAsync(
            StatusConta.Pendente, inicio, fim, clienteId: null,
            diasAtrasoMinimo: null, skip: 0, take: 200, ct);
        return candidatas.FirstOrDefault(c => c.Saldo == item.Valor);
    }
}
