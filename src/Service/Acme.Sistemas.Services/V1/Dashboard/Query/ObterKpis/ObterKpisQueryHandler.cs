using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;

public sealed class ObterKpisQueryHandler
    : IRequestHandler<ObterKpisQuery, ResponseDefault<ObterKpisQueryResult>>
{
    private readonly IDespesaRepository _despesas;
    private readonly IReceitaRepository _receitas;
    private readonly IDashboardRepository _dashboard;

    public ObterKpisQueryHandler(
        IDespesaRepository despesas,
        IReceitaRepository receitas,
        IDashboardRepository dashboard)
    {
        _despesas = despesas;
        _receitas = receitas;
        _dashboard = dashboard;
    }

    public async Task<ResponseDefault<ObterKpisQueryResult>> Handle(ObterKpisQuery request, CancellationToken cancellationToken)
    {
        var hoje = DateTime.UtcNow;
        var inicio = request.Inicio ?? new DateTime(hoje.Year, hoje.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var fim = request.Fim ?? inicio.AddMonths(1).AddTicks(-1);

        var receita = await _receitas.SumByPeriodoAsync(inicio, fim, somenteRecebidas: true, cancellationToken);
        var despesa = await _despesas.SumByPeriodoAsync(inicio, fim, somenteBaixadas: true, cancellationToken);
        var vendas = await _dashboard.CountVendasAbertasAsync(cancellationToken);
        var crVenc = await _dashboard.CountContasReceberVencendoAsync(7, cancellationToken);
        var cpVenc = await _dashboard.CountContasPagarVencendoAsync(7, cancellationToken);
        var critico = await _dashboard.CountProdutosEmEstoqueCriticoAsync(cancellationToken);

        return ResponseDefault<ObterKpisQueryResult>.Ok(new ObterKpisQueryResult(
            inicio, fim, receita, despesa, receita - despesa,
            vendas, crVenc, cpVenc, critico));
    }
}
