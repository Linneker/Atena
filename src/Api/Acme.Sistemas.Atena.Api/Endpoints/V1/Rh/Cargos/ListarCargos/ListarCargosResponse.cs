namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cargos.ListarCargos;

public sealed record ListarCargosResponseItem(
    Guid Id,
    string? Codigo,
    string Descricao,
    string? CodigoCbo,
    decimal? SalarioBaseSugerido,
    bool Ativo);

public sealed record ListarCargosResponse(
    IReadOnlyList<ListarCargosResponseItem> Items,
    long Total);
