using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using CboEntity = Acme.Sistemas.Domain.Entities.Referencia.Cbo;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Command.SeedCbos;

public sealed class SeedCbosCommandHandler
    : IRequestHandler<SeedCbosCommand, ResponseDefault<SeedCbosCommandResult>>
{
    private readonly ICboRepository _repo;

    public SeedCbosCommandHandler(ICboRepository repo) => _repo = repo;

    public async Task<ResponseDefault<SeedCbosCommandResult>> Handle(
        SeedCbosCommand request, CancellationToken cancellationToken)
    {
        var cbos = request.Cbos
            .Select(c => new CboEntity
            {
                Codigo = c.Codigo,
                Titulo = c.Titulo,
                GrandeGrupo = c.GrandeGrupo,
                Familia = c.Familia,
                Ativo = true,
            })
            .ToList();

        var upserted = await _repo.UpsertManyAsync(cbos, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        return ResponseDefault<SeedCbosCommandResult>.Ok(
            new SeedCbosCommandResult(upserted, total));
    }
}
