namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ObterCargo;

public sealed record ObterCargoQueryResult(
    Guid Id,
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido,
    bool Ativo);
