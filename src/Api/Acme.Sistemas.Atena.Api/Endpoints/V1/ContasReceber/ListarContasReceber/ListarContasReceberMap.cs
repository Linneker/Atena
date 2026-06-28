using Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ListarContasReceber;

public static class ListarContasReceberMap
{
    public static ListarContasReceberQuery ToQuery(this ListarContasReceberRequest request)
        => new(request.Status, request.VencimentoInicio, request.VencimentoFim, request.ClienteId,
            request.DiasAtrasoMinimo, request.Skip ?? 0, request.Take ?? 50);

    public static ListarContasReceberResponse ToResponse(this ListarContasReceberQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total, result.Skip, result.Take);

    private static ListarContasReceberResponseItem ToResponseItem(this ListarContasReceberQueryItem item)
        => new(item.Id, item.Descricao, item.ClienteId, item.ClienteNome,
            item.ValorOriginal, item.ValorRecebido,
            item.Saldo, item.DataVencimento, item.Status, item.DiasAtraso);
}
