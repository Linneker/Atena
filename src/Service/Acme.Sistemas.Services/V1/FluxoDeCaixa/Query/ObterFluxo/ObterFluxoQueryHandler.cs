using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

public sealed class ObterFluxoQueryHandler
    : IRequestHandler<ObterFluxoQuery, ResponseDefault<ObterFluxoQueryResult>>
{
    private readonly IDespesaRepository _despesas;
    private readonly IReceitaRepository _receitas;
    private readonly IFechamentoPeriodoRepository _fechamentos;

    public ObterFluxoQueryHandler(
        IDespesaRepository despesas,
        IReceitaRepository receitas,
        IFechamentoPeriodoRepository fechamentos)
    {
        _despesas = despesas;
        _receitas = receitas;
        _fechamentos = fechamentos;
    }

    public async Task<ResponseDefault<ObterFluxoQueryResult>> Handle(
        ObterFluxoQuery request,
        CancellationToken cancellationToken)
    {
        var totalDespesas = await _despesas.SumByPeriodoAsync(
            request.Inicio, request.Fim, request.SomenteRealizados, cancellationToken);

        var totalReceitas = await _receitas.SumByPeriodoAsync(
            request.Inicio, request.Fim, request.SomenteRealizados, cancellationToken);

        var fechado = false;
        if (request.Inicio.Year == request.Fim.Year && request.Inicio.Month == request.Fim.Month)
        {
            var fechamento = await _fechamentos.GetByPeriodoAsync(
                request.Inicio.Year, request.Inicio.Month, cancellationToken);
            fechado = fechamento is not null;
        }

        // Detalhamento: lista todas Despesas e Receitas do período (até 5000 itens).
        var statusFiltro = request.SomenteRealizados ? (StatusPagamento?)StatusPagamento.Pago : null;

        var despesas = await _despesas.ListByFiltroAsync(
            statusFiltro, request.Inicio, request.Fim, null, null, 0, 5000, cancellationToken);

        var receitas = await _receitas.ListByFiltroAsync(
            statusFiltro, request.Inicio, request.Fim, null, null, 0, 5000, cancellationToken);

        var movimentos = new List<FluxoMovimentoItem>(despesas.Count + receitas.Count);

        foreach (var d in despesas)
        {
            movimentos.Add(new FluxoMovimentoItem(
                d.DataPagamento ?? d.DataVencimento,
                "Despesa",
                d.Nome,
                d.ValorPago ?? d.Valor,
                d.StatusPagamento.ToString(),
                d.StatusPagamento == StatusPagamento.Pago));
        }

        foreach (var r in receitas)
        {
            movimentos.Add(new FluxoMovimentoItem(
                r.DataRecebimento ?? r.DataPrevistaRecebimento,
                "Receita",
                r.Nome,
                r.ValorRecebido ?? r.Valor,
                r.StatusRecebimento.ToString(),
                r.StatusRecebimento == StatusPagamento.Pago));
        }

        movimentos = movimentos.OrderBy(m => m.Data).ThenBy(m => m.Tipo).ToList();

        return ResponseDefault<ObterFluxoQueryResult>.Ok(new ObterFluxoQueryResult(
            request.Inicio,
            request.Fim,
            totalReceitas,
            totalDespesas,
            totalReceitas - totalDespesas,
            request.SomenteRealizados,
            fechado,
            movimentos));
    }
}
