using Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ListarDespesas;

public static class ListarDespesasMap
{
    public static ListarDespesasQuery ToQuery(this ListarDespesasRequest request)
        => new(
            request.Status,
            request.VencimentoInicio,
            request.VencimentoFim,
            request.Categoria,
            request.CompetenciaId,
            request.Skip,
            request.Take);

    public static ListarDespesasResponse ToResponse(this ListarDespesasQueryResult result)
        => new(
            result.Items.Select(i => i.ToResponseItem()).ToArray(),
            result.Total,
            result.Skip,
            result.Take);

    private static ListarDespesasResponseItem ToResponseItem(this ListarDespesasQueryItem item)
        => new(
            item.Id,
            item.Nome,
            item.Categoria,
            item.Valor,
            item.DataVencimento,
            item.StatusPagamento,
            item.ValorPago,
            item.DataPagamento,
            item.CompetenciaId,
            item.CentroDeCustoId,
            item.CentroDeCustoNome,
            item.FornecedorId,
            item.DespesaFixa);
}
