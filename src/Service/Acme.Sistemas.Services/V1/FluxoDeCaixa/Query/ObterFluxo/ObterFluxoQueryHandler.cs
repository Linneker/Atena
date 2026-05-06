using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
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

        return ResponseDefault<ObterFluxoQueryResult>.Ok(new ObterFluxoQueryResult(
            request.Inicio,
            request.Fim,
            totalReceitas,
            totalDespesas,
            totalReceitas - totalDespesas,
            request.SomenteRealizados,
            fechado));
    }
}
