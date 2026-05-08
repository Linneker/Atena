using Acme.Sistemas.Services.V1.ContaPagar.Command.CriarContaPagar;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.CriarContaPagar;

public static class CriarContaPagarMap
{
    public static CriarContaPagarCommand ToCommand(this CriarContaPagarRequest request)
        => new(request.Descricao, request.FornecedorId, request.DespesaId, request.PlanoDeContasId,
            request.ValorOriginal, request.DataVencimento, request.Observacao);

    public static CriarContaPagarResponse ToResponse(this CriarContaPagarCommandResult result)
        => new(result.Id, result.Descricao, result.ValorOriginal, result.DataVencimento);
}
