using Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ObterSolicitacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.ObterSolicitacao;

public static class ObterSolicitacaoMap
{
    public static ObterSolicitacaoQuery ToQuery(this ObterSolicitacaoRequest request)
        => new(request.Id);

    public static ObterSolicitacaoResponse ToResponse(this ObterSolicitacaoQueryResult result)
        => new(
            result.Id,
            result.Numero,
            result.SolicitanteId,
            result.Justificativa,
            result.ValorTotal,
            result.DataSolicitacao,
            result.Status,
            result.AprovadoPor,
            result.AprovadoEm,
            result.MotivoRejeicao,
            result.Itens.Select(i => i.ToResponseItem()).ToArray());

    private static ObterSolicitacaoResponseItem ToResponseItem(this SolicitacaoItemView item)
        => new(item.Id, item.ProdutoId, item.Quantidade, item.PrecoEstimado, item.Observacao);
}
