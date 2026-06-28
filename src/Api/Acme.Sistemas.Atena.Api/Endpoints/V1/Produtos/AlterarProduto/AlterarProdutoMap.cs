using Acme.Sistemas.Services.V1.Produto.Command.AlterarProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.AlterarProduto;

public static class AlterarProdutoMap
{
    public static AlterarProdutoCommand ToCommand(this AlterarProdutoRequest request, Guid id)
        => new(
            id,
            request.Nome,
            request.Descricao,
            request.CodigoBarras,
            request.UnidadeMedida,
            request.TipoProdutoId,
            request.FornecedorId,
            request.CustoMedio,
            request.EstoqueMinimo,
            request.Status);

    public static AlterarProdutoResponse ToResponse(this AlterarProdutoCommandResult result)
        => new(result.Id);
}
