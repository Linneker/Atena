namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Balanco.GerarBalanco;

public sealed record BalancoLinhaResponse(string Descricao, decimal Valor);

public sealed record GerarBalancoResponse(
    DateTime DataReferencia,
    IReadOnlyList<BalancoLinhaResponse> Ativo,
    IReadOnlyList<BalancoLinhaResponse> Passivo,
    IReadOnlyList<BalancoLinhaResponse> PatrimonioLiquido,
    decimal TotalAtivo,
    decimal TotalPassivo,
    decimal TotalPatrimonioLiquido);
