using Acme.Sistemas.Services.V1.Receita.Command.ReceberReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ReceberReceita;

public static class ReceberReceitaMap
{
    public static ReceberReceitaCommand ToCommand(this ReceberReceitaRequest request, Guid id)
        => new(id, request.ValorRecebido, request.DataRecebimento, request.FormaPagamento, request.Observacao);

    public static ReceberReceitaResponse ToResponse(this ReceberReceitaCommandResult result)
        => new(result.Id, result.StatusRecebimento, result.ValorRecebido, result.DataRecebimento);
}
