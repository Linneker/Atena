using Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.ObterContaPagar;

public static class ObterContaPagarMap
{
    public static ObterContaPagarQuery ToQuery(this ObterContaPagarRequest request)
        => new(request.Id);

    public static ObterContaPagarResponse ToResponse(this ObterContaPagarQueryResult result)
        => new(result.Id, result.Descricao, result.FornecedorId, result.DespesaId, result.PlanoDeContasId,
            result.ValorOriginal, result.ValorPago, result.Saldo, result.DataVencimento, result.DataPagamento,
            result.Status, result.Observacao, result.CreatedAt);
}
