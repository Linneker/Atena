using Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ObterReceita;

public static class ObterReceitaMap
{
    public static ObterReceitaQuery ToQuery(this ObterReceitaRequest request)
        => new(request.Id);

    public static ObterReceitaResponse ToResponse(this ObterReceitaQueryResult result)
        => new(
            result.Id,
            result.Nome,
            result.Descricao,
            result.Categoria,
            result.Valor,
            result.ReceitaFixa,
            result.DataPrevistaRecebimento,
            result.CompetenciaId,
            result.CentroDeCustoId,
            result.CentroDeCustoNome,
            result.ClienteId,
            result.OrigemVendaId,
            result.StatusRecebimento,
            result.ValorRecebido,
            result.DataRecebimento,
            result.FormaPagamento,
            result.ObservacaoRecebimento,
            result.CreatedAt,
            result.UpdatedAt);
}
