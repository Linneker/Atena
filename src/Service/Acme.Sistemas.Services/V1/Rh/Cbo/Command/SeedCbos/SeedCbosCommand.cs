using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Command.SeedCbos;

public sealed record SeedCbosCommandItem(
    string Codigo,
    string Titulo,
    string? GrandeGrupo,
    string? Familia);

/// <summary>
/// Seed (upsert) do catálogo CBO. Endpoint admin opt-in — substitui o conteúdo
/// para os códigos enviados (não trunca os demais).
/// </summary>
public sealed record SeedCbosCommand(IReadOnlyList<SeedCbosCommandItem> Cbos)
    : IRequest<ResponseDefault<SeedCbosCommandResult>>;
