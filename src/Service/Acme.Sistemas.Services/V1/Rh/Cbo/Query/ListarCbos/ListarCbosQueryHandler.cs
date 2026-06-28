using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Query.ListarCbos;

public sealed class ListarCbosQueryHandler
    : IRequestHandler<ListarCbosQuery, ResponseDefault<ListarCbosQueryResult>>
{
    private readonly ICboRepository _repo;

    public ListarCbosQueryHandler(ICboRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarCbosQueryResult>> Handle(
        ListarCbosQuery request, CancellationToken cancellationToken)
    {
        var cbos = await _repo.ListAllAsync(cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);

        var items = cbos
            .Select(c => new ListarCbosQueryItem(c.Codigo, c.Titulo, c.GrandeGrupo, c.Familia))
            .ToList();

        return ResponseDefault<ListarCbosQueryResult>.Ok(
            new ListarCbosQueryResult(items, total));
    }
}
