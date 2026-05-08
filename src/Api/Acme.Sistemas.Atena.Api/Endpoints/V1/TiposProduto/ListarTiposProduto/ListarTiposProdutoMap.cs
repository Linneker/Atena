using Acme.Sistemas.Services.V1.TipoProduto.Query.ListarTiposProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposProduto.ListarTiposProduto;

public static class ListarTiposProdutoMap
{
    public static ListarTiposProdutoQuery ToQuery(this ListarTiposProdutoRequest _) => new();

    public static ListarTiposProdutoResponse ToResponse(this ListarTiposProdutoQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray());

    private static ListarTiposProdutoResponseItem ToResponseItem(this ListarTiposProdutoQueryItem item)
        => new(item.Id, item.Nome, item.Descricao, item.Ativo);
}
