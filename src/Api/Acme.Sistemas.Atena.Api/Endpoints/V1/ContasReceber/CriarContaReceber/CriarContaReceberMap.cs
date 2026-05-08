using Acme.Sistemas.Services.V1.ContaReceber.Command.CriarContaReceber;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.CriarContaReceber;

public static class CriarContaReceberMap
{
    public static CriarContaReceberCommand ToCommand(this CriarContaReceberRequest request)
        => new(request.Descricao, request.ClienteId, request.ReceitaId, request.PlanoDeContasId,
            request.ValorOriginal, request.DataVencimento);

    public static CriarContaReceberResponse ToResponse(this CriarContaReceberCommandResult result)
        => new(result.Id, result.Descricao, result.ValorOriginal, result.DataVencimento);
}
