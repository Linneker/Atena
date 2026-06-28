using Acme.Sistemas.Services.V1.Orcamento.Query.ListarOrcamentos;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.ListarOrcamentos;

public static class ListarOrcamentosMap
{
    public static ListarOrcamentosQuery ToQuery(this ListarOrcamentosRequest request)
        => new(request.Status, request.ClienteId, request.Skip, request.Take);

    public static ListarOrcamentosResponse ToResponse(this ListarOrcamentosQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total);

    private static ListarOrcamentosResponseItem ToResponseItem(this ListarOrcamentosQueryItem item)
        => new(
            item.Id,
            item.Numero,
            item.ClienteId,
            item.VendedorId,
            item.DataEmissao,
            item.DataValidade,
            item.ValorTotal,
            item.Status);
}
