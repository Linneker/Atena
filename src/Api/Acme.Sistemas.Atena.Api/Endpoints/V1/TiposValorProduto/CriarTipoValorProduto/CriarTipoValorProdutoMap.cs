using Acme.Sistemas.Services.V1.TipoValorProduto.Command.CriarTipoValorProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposValorProduto.CriarTipoValorProduto;

public static class CriarTipoValorProdutoMap
{
    public static CriarTipoValorProdutoCommand ToCommand(this CriarTipoValorProdutoRequest request)
        => new(request.Nome, request.Descricao);

    public static CriarTipoValorProdutoResponse ToResponse(this CriarTipoValorProdutoCommandResult result)
        => new(result.Id, result.Nome);
}
