using Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ObterContaReceber;

public static class ObterContaReceberMap
{
    public static ObterContaReceberQuery ToQuery(this ObterContaReceberRequest request)
        => new(request.Id);

    public static ObterContaReceberResponse ToResponse(this ObterContaReceberQueryResult result)
        => new(result.Id, result.Descricao, result.ClienteId, result.ReceitaId, result.PlanoDeContasId,
            result.ValorOriginal, result.ValorRecebido, result.Saldo, result.DataVencimento,
            result.DataRecebimento, result.Status, result.DiasAtraso, result.ObservacaoRecebimento,
            result.CreatedAt);
}
