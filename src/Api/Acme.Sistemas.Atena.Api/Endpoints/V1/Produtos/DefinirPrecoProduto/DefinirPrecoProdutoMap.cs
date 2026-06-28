using Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.DefinirPrecoProduto;

public static class DefinirPrecoProdutoMap
{
    public static DefinirPrecoProdutoCommand ToCommand(this DefinirPrecoProdutoRequest request, Guid produtoId)
        => new(produtoId, request.TipoValorProdutoId, request.Valor, request.VigenciaInicio);

    public static DefinirPrecoProdutoResponse ToResponse(this DefinirPrecoProdutoCommandResult result)
        => new(result.PrecoId, result.Valor, result.VigenciaInicio);
}
