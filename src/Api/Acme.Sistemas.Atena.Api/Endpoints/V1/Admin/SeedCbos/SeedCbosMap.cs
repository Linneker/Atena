using Acme.Sistemas.Services.V1.Rh.Cbo.Command.SeedCbos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Admin.SeedCbos;

public static class SeedCbosMap
{
    public static SeedCbosCommand ToCommand(this SeedCbosRequest r)
        => new(r.Cbos.Select(i => new SeedCbosCommandItem(
            i.Codigo, i.Titulo, i.GrandeGrupo, i.Familia)).ToList());

    public static SeedCbosResponse ToResponse(this SeedCbosCommandResult r)
        => new(r.Upserted, r.TotalAposSeed);
}
