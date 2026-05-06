using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Query.ListarTiposValorProduto;

public sealed class ListarTiposValorProdutoQueryHandler
    : IRequestHandler<ListarTiposValorProdutoQuery, ResponseDefault<ListarTiposValorProdutoQueryResult>>
{
    private readonly ITipoValorProdutoRepository _repo;

    public ListarTiposValorProdutoQueryHandler(ITipoValorProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarTiposValorProdutoQueryResult>> Handle(ListarTiposValorProdutoQuery request, CancellationToken cancellationToken)
    {
        var tipos = await _repo.ListAsync(0, 500, cancellationToken);
        var items = tipos.Select(t => new ListarTiposValorProdutoQueryItem(t.Id, t.Nome, t.Descricao, t.Ativo)).ToList();
        return ResponseDefault<ListarTiposValorProdutoQueryResult>.Ok(new ListarTiposValorProdutoQueryResult(items));
    }
}
