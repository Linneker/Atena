using Acme.Sistemas.Services.V1.Produto.Query.ListarProdutos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.ListarProdutos;

public static class ListarProdutosMap
{
    public static ListarProdutosQuery ToQuery(this ListarProdutosRequest request)
        => new(request.Termo, request.Skip, request.Take);

    public static ListarProdutosResponse ToResponse(this ListarProdutosQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total);

    private static ListarProdutosResponseItem ToResponseItem(this ListarProdutosQueryItem item)
        => new(
            item.Id,
            item.Codigo,
            item.Nome,
            item.CodigoBarras,
            item.UnidadeMedida,
            item.CustoMedio,
            item.Status);
}
