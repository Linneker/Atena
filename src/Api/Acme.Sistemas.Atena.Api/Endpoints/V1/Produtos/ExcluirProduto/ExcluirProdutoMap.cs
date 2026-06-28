using Acme.Sistemas.Services.V1.Produto.Command.ExcluirProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.ExcluirProduto;

public static class ExcluirProdutoMap
{
    public static ExcluirProdutoCommand ToCommand(this ExcluirProdutoRequest request)
        => new(request.Id);
}
