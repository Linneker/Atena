using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Uf.Query.ListarUfs;

public sealed class ListarUfsQueryHandler
    : IRequestHandler<ListarUfsQuery, ResponseDefault<ListarUfsQueryResult>>
{
    private readonly IUfRepository _repo;

    public ListarUfsQueryHandler(IUfRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarUfsQueryResult>> Handle(
        ListarUfsQuery request, CancellationToken cancellationToken)
    {
        var ufs = await _repo.ListAllAsync(cancellationToken);
        var items = ufs.Select(u => new ListarUfsQueryItem(u.Sigla, u.Nome, u.CodigoIbge)).ToList();
        return ResponseDefault<ListarUfsQueryResult>.Ok(new ListarUfsQueryResult(items));
    }
}
