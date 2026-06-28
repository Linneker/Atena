using Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.ObterProduto;

public static class ObterProdutoMap
{
    public static ObterProdutoQuery ToQuery(this ObterProdutoRequest request)
        => new(request.Id);

    public static ObterProdutoResponse ToResponse(this ObterProdutoQueryResult result)
        => new(
            result.Id,
            result.Codigo,
            result.Nome,
            result.Descricao,
            result.CodigoBarras,
            result.UnidadeMedida,
            result.TipoProdutoId,
            result.FornecedorId,
            result.FornecedorNome,
            result.CustoMedio,
            result.EstoqueMinimo,
            result.Status,
            result.Precos.Select(p => p.ToResponsePreco()).ToArray());

    private static ObterProdutoResponsePreco ToResponsePreco(this PrecoVigente preco)
        => new(preco.Id, preco.TipoValorProdutoId, preco.Valor, preco.VigenciaInicio, preco.VigenciaFim);
}
