namespace Acme.Sistemas.Domain.Reports;

public sealed record BalancoLinha(string Descricao, decimal Valor);

public sealed record BalancoResult(
    DateTime DataReferencia,
    IReadOnlyList<BalancoLinha> Ativo,
    IReadOnlyList<BalancoLinha> Passivo,
    IReadOnlyList<BalancoLinha> PatrimonioLiquido,
    decimal TotalAtivo,
    decimal TotalPassivo,
    decimal TotalPatrimonioLiquido);
