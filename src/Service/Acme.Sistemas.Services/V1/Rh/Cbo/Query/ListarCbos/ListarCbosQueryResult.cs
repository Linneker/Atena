namespace Acme.Sistemas.Services.V1.Rh.Cbo.Query.ListarCbos;

public sealed record ListarCbosQueryItem(
    string Codigo,
    string Titulo,
    string? GrandeGrupo,
    string? Familia);

public sealed record ListarCbosQueryResult(
    IReadOnlyList<ListarCbosQueryItem> Items,
    long Total);
