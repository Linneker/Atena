using Acme.Sistemas.Services.V1.SolicitacaoCompra.Query.ListarSolicitacoes;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.ListarSolicitacoes;

public static class ListarSolicitacoesMap
{
    public static ListarSolicitacoesQuery ToQuery(this ListarSolicitacoesRequest request)
        => new(request.Status, request.Skip, request.Take);

    public static ListarSolicitacoesResponse ToResponse(this ListarSolicitacoesQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total);

    private static ListarSolicitacoesResponseItem ToResponseItem(this ListarSolicitacoesQueryItem item)
        => new(item.Id, item.Numero, item.SolicitanteId, item.ValorTotal, item.DataSolicitacao, item.Status);
}
