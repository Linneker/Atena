namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Cbos.ListarCbos;

public sealed record ListarCbosResponseItem(
    string Codigo,
    string Titulo,
    string? GrandeGrupo,
    string? Familia);

public sealed record ListarCbosResponse(
    IReadOnlyList<ListarCbosResponseItem> Items,
    long Total);
