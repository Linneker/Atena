using Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.CriarProduto;

public static class CriarProdutoMap
{
    public static CriarProdutoCommand ToCommand(this CriarProdutoRequest request)
        => new(
            request.Codigo,
            request.Nome,
            request.Descricao,
            request.CodigoBarras,
            request.UnidadeMedida,
            request.TipoProdutoId,
            request.FornecedorId,
            request.CustoMedio,
            request.EstoqueMinimo);

    public static CriarProdutoResponse ToResponse(this CriarProdutoCommandResult result)
        => new(result.Id, result.Codigo, result.Nome);
}
