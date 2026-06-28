namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

public sealed record FluxoMovimentoItem(
    DateTime Data,
    string Tipo,           // "Receita" | "Despesa"
    string Descricao,
    decimal Valor,
    string Status,
    bool Realizado);

public sealed record ObterFluxoQueryResult(
    DateTime Inicio,
    DateTime Fim,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Resultado,
    bool SomenteRealizados,
    bool PeriodoFechado,
    IReadOnlyList<FluxoMovimentoItem> Movimentos);
