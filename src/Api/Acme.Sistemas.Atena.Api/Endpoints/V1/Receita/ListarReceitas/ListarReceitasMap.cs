using Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ListarReceitas;

public static class ListarReceitasMap
{
    public static ListarReceitasQuery ToQuery(this ListarReceitasRequest request)
        => new(
            request.Status,
            request.RecebimentoInicio,
            request.RecebimentoFim,
            request.Categoria,
            request.CompetenciaId,
            request.Skip,
            request.Take);

    public static ListarReceitasResponse ToResponse(this ListarReceitasQueryResult result)
        => new(
            result.Items.Select(i => i.ToResponseItem()).ToArray(),
            result.Total,
            result.Skip,
            result.Take);

    private static ListarReceitasResponseItem ToResponseItem(this ListarReceitasQueryItem item)
        => new(
            item.Id,
            item.Nome,
            item.Categoria,
            item.Valor,
            item.DataPrevistaRecebimento,
            item.StatusRecebimento,
            item.ValorRecebido,
            item.DataRecebimento,
            item.CompetenciaId,
            item.CentroDeCustoId,
            item.CentroDeCustoNome,
            item.ClienteId,
            item.OrigemVendaId,
            item.ReceitaFixa);
}
