using Acme.Sistemas.Services.V1.Despesa.Query.ObterDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ObterDespesa;

public static class ObterDespesaMap
{
    public static ObterDespesaQuery ToQuery(this ObterDespesaRequest request)
        => new(request.Id);

    public static ObterDespesaResponse ToResponse(this ObterDespesaQueryResult result)
        => new(
            result.Id,
            result.Nome,
            result.Descricao,
            result.Categoria,
            result.Valor,
            result.DespesaFixa,
            result.DataVencimento,
            result.CompetenciaId,
            result.CentroDeCustoId,
            result.CentroDeCustoNome,
            result.FornecedorId,
            result.StatusPagamento,
            result.ValorPago,
            result.DataPagamento,
            result.FormaPagamento,
            result.ObservacaoPagamento,
            result.CreatedAt,
            result.UpdatedAt);
}
