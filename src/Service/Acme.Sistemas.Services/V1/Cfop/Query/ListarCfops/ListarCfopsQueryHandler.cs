using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Cfop.Query.ListarCfops;

public sealed class ListarCfopsQueryHandler
    : IRequestHandler<ListarCfopsQuery, ResponseDefault<ListarCfopsQueryResult>>
{
    private readonly ICfopRepository _repo;

    public ListarCfopsQueryHandler(ICfopRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarCfopsQueryResult>> Handle(
        ListarCfopsQuery request, CancellationToken cancellationToken)
    {
        var cfops = await _repo.ListAsync(request.Categoria, cancellationToken);
        var items = cfops.Select(c => new ListarCfopsQueryItem(c.Codigo, c.Descricao, c.Categoria)).ToList();
        return ResponseDefault<ListarCfopsQueryResult>.Ok(new ListarCfopsQueryResult(items));
    }
}
