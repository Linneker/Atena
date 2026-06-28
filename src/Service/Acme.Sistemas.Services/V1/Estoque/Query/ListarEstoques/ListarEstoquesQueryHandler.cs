using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ListarEstoques;

public sealed class ListarEstoquesQueryHandler
    : IRequestHandler<ListarEstoquesQuery, ResponseDefault<ListarEstoquesQueryResult>>
{
    private readonly IEstoqueRepository _repo;

    public ListarEstoquesQueryHandler(IEstoqueRepository repo) { _repo = repo; }

    public async Task<ResponseDefault<ListarEstoquesQueryResult>> Handle(
        ListarEstoquesQuery request, CancellationToken cancellationToken)
    {
        var estoques = await _repo.ListAsync(request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(cancellationToken);
        var items = estoques.Select(e => new ListarEstoquesQueryItem(
            e.Id, e.Codigo, e.Nome, e.Localizacao, e.Ativo)).ToList();
        return ResponseDefault<ListarEstoquesQueryResult>.Ok(
            new ListarEstoquesQueryResult(items, total));
    }
}
