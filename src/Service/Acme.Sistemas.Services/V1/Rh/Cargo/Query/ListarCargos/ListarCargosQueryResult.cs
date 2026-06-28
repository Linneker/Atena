namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ListarCargos;

public sealed record ListarCargosQueryItem(
    Guid Id,
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido,
    bool Ativo);

public sealed record ListarCargosQueryResult(
    IReadOnlyList<ListarCargosQueryItem> Items,
    long Total);
