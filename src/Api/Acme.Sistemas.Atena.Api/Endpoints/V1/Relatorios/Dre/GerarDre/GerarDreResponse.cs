namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Dre.GerarDre;

public sealed record DreLinhaResponse(
    Guid PlanoId,
    string Codigo,
    string Nome,
    int Nivel,
    decimal Valor,
    decimal Total,
    IReadOnlyList<DreLinhaResponse> Filhos);

public sealed record GerarDreResponse(
    DateTime Inicio,
    DateTime Fim,
    IReadOnlyList<DreLinhaResponse> Receitas,
    IReadOnlyList<DreLinhaResponse> Despesas,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal ResultadoLiquido);
