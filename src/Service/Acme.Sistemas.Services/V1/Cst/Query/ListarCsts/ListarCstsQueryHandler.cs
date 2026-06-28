using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cst.Query.ListarCsts;

public sealed class ListarCstsQueryHandler
    : IRequestHandler<ListarCstsQuery, ResponseDefault<ListarCstsQueryResult>>
{
    private readonly ICstRepository _repo;

    public ListarCstsQueryHandler(ICstRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarCstsQueryResult>> Handle(
        ListarCstsQuery request, CancellationToken cancellationToken)
    {
        var csts = await _repo.ListByTipoAsync(request.Tipo, cancellationToken);
        var items = csts.Select(c => new ListarCstsQueryItem(c.Codigo, c.Descricao)).ToList();
        return ResponseDefault<ListarCstsQueryResult>.Ok(
            new ListarCstsQueryResult(request.Tipo.ToLowerInvariant(), items));
    }
}
