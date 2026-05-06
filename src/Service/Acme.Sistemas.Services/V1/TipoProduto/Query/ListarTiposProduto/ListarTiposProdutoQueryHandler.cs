using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.TipoProduto.Query.ListarTiposProduto;

public sealed class ListarTiposProdutoQueryHandler
    : IRequestHandler<ListarTiposProdutoQuery, ResponseDefault<ListarTiposProdutoQueryResult>>
{
    private readonly ITipoProdutoRepository _repo;

    public ListarTiposProdutoQueryHandler(ITipoProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarTiposProdutoQueryResult>> Handle(ListarTiposProdutoQuery request, CancellationToken cancellationToken)
    {
        var tipos = await _repo.ListAsync(0, 500, cancellationToken);
        var items = tipos.Select(t => new ListarTiposProdutoQueryItem(t.Id, t.Nome, t.Descricao, t.Ativo)).ToList();
        return ResponseDefault<ListarTiposProdutoQueryResult>.Ok(new ListarTiposProdutoQueryResult(items));
    }
}
