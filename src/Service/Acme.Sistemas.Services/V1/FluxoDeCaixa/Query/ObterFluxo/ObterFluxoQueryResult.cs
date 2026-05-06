namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

public sealed record ObterFluxoQueryResult(
    DateTime Inicio,
    DateTime Fim,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Resultado,
    bool SomenteRealizados,
    bool PeriodoFechado);
