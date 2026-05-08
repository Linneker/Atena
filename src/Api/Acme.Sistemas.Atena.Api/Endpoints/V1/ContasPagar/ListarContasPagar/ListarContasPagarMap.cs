using Acme.Sistemas.Services.V1.ContaPagar.Query.ListarContasPagar;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.ListarContasPagar;

public static class ListarContasPagarMap
{
    public static ListarContasPagarQuery ToQuery(this ListarContasPagarRequest request)
        => new(request.Status, request.VencimentoInicio, request.VencimentoFim, request.FornecedorId,
            request.VencendoEmAteSeteDias ?? false, request.Skip ?? 0, request.Take ?? 50);

    public static ListarContasPagarResponse ToResponse(this ListarContasPagarQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total, result.Skip, result.Take);

    private static ListarContasPagarResponseItem ToResponseItem(this ListarContasPagarQueryItem item)
        => new(item.Id, item.Descricao, item.FornecedorId, item.ValorOriginal, item.ValorPago,
            item.Saldo, item.DataVencimento, item.Status, item.Vencida, item.DiasParaVencer);
}
