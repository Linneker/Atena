using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Reports;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

/// <summary>
/// Balanço patrimonial gerencial simplificado:
/// - Ativo: contas a receber pendentes (capital de giro a entrar)
/// - Passivo: contas a pagar pendentes + dívidas em aberto
/// - PL: Ativo - Passivo (resultado acumulado gerencial)
/// Para um Balanço Contábil completo é necessário um livro razão de
/// lançamentos por partida dobrada — fora do escopo desta fase.
/// </summary>
public sealed class GerarBalancoQueryHandler
    : IRequestHandler<GerarBalancoQuery, ResponseDefault<BalancoResult>>
{
    private readonly IRelatoriosFinanceirosRepository _agg;

    public GerarBalancoQueryHandler(IRelatoriosFinanceirosRepository agg)
    {
        _agg = agg;
    }

    public async Task<ResponseDefault<BalancoResult>> Handle(GerarBalancoQuery request, CancellationToken cancellationToken)
    {
        var contasReceberPendentes = await _agg.TotalContasReceberPendentesAsync(cancellationToken);
        var contasPagarPendentes = await _agg.TotalContasPagarPendentesAsync(cancellationToken);
        var dividasAbertas = await _agg.TotalDividasAbertasAsync(cancellationToken);

        var ativo = new List<BalancoLinha>
        {
            new("Contas a Receber (pendentes)", contasReceberPendentes)
        };
        var passivo = new List<BalancoLinha>
        {
            new("Contas a Pagar (pendentes)", contasPagarPendentes),
            new("Dívidas em aberto", dividasAbertas)
        };

        var totalAtivo = ativo.Sum(l => l.Valor);
        var totalPassivo = passivo.Sum(l => l.Valor);
        var resultadoAcumulado = totalAtivo - totalPassivo;

        var pl = new List<BalancoLinha>
        {
            new("Resultado Gerencial Acumulado", resultadoAcumulado)
        };

        return ResponseDefault<BalancoResult>.Ok(new BalancoResult(
            request.DataReferencia,
            ativo, passivo, pl,
            totalAtivo, totalPassivo, resultadoAcumulado));
    }
}
