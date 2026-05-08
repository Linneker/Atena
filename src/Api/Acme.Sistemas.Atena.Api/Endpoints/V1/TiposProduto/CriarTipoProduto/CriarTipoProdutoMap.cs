using Acme.Sistemas.Services.V1.TipoProduto.Command.CriarTipoProduto;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposProduto.CriarTipoProduto;

public static class CriarTipoProdutoMap
{
    public static CriarTipoProdutoCommand ToCommand(this CriarTipoProdutoRequest request)
        => new(request.Nome, request.Descricao);

    public static CriarTipoProdutoResponse ToResponse(this CriarTipoProdutoCommandResult result)
        => new(result.Id, result.Nome);
}
