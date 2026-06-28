namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.ObterFluxo;

public sealed record FluxoMovimentoResponseItem(
    DateTime Data,
    string Tipo,
    string Descricao,
    decimal Valor,
    string Status,
    bool Realizado);

public sealed record ObterFluxoResponse(
    DateTime Inicio,
    DateTime Fim,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Resultado,
    bool SomenteRealizados,
    bool PeriodoFechado,
    IReadOnlyList<FluxoMovimentoResponseItem> Movimentos);
