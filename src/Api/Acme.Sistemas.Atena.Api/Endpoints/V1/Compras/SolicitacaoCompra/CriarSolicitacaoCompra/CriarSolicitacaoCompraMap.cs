using Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.CriarSolicitacaoCompra;

public static class CriarSolicitacaoCompraMap
{
    public static CriarSolicitacaoCommand ToCommand(this CriarSolicitacaoCompraRequest request)
        => new(
            request.Justificativa,
            request.Itens.Select(i => new SolicitacaoItemDto(i.ProdutoId, i.Quantidade, i.PrecoEstimado, i.Observacao)).ToArray(),
            request.EnviarParaAprovacao);

    public static CriarSolicitacaoCompraResponse ToResponse(this CriarSolicitacaoCommandResult result)
        => new(result.Id, result.Numero, result.ValorTotal);
}
