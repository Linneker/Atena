using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Produto.Query.ListarProdutos;

public sealed class ListarProdutosQueryHandler
    : IRequestHandler<ListarProdutosQuery, ResponseDefault<ListarProdutosQueryResult>>
{
    private readonly IProdutoRepository _repo;

    public ListarProdutosQueryHandler(IProdutoRepository repo)
    {
        _repo = repo;
    }

    public async Task<ResponseDefault<ListarProdutosQueryResult>> Handle(ListarProdutosQuery request, CancellationToken cancellationToken)
    {
        var produtos = await _repo.ListByFiltroAsync(request.Termo, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountByFiltroAsync(request.Termo, cancellationToken);

        var items = produtos.Select(p => new ListarProdutosQueryItem(
            p.Id, p.Codigo, p.Nome, p.CodigoBarras,
            p.UnidadeMedida, p.CustoMedio, p.Status)).ToList();

        return ResponseDefault<ListarProdutosQueryResult>.Ok(
            new ListarProdutosQueryResult(items, total));
    }
}
