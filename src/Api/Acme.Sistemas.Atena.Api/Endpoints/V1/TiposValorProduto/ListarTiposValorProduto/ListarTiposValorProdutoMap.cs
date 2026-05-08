using Acme.Sistemas.Services.V1.TipoValorProduto.Query.ListarTiposValorProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposValorProduto.ListarTiposValorProduto;

public static class ListarTiposValorProdutoMap
{
    public static ListarTiposValorProdutoQuery ToQuery(this ListarTiposValorProdutoRequest _) => new();

    public static ListarTiposValorProdutoResponse ToResponse(this ListarTiposValorProdutoQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray());

    private static ListarTiposValorProdutoResponseItem ToResponseItem(this ListarTiposValorProdutoQueryItem item)
        => new(item.Id, item.Nome, item.Descricao, item.Ativo);
}
