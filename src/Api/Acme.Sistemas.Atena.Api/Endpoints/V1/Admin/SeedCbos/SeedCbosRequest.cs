namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.SeedCbos;

public sealed record SeedCbosRequestItem(
    string Codigo,
    string Titulo,
    string? GrandeGrupo,
    string? Familia);

public sealed record SeedCbosRequest(IReadOnlyList<SeedCbosRequestItem> Cbos);
