using Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ReceberContaReceber;

public static class ReceberContaReceberMap
{
    public static ReceberContaReceberCommand ToCommand(this ReceberContaReceberRequest request, Guid id)
        => new(id, request.ValorRecebido, request.DataRecebimento, request.Observacao);

    public static ReceberContaReceberResponse ToResponse(this ReceberContaReceberCommandResult result)
        => new(result.Id, result.Status, result.ValorRecebido, result.Saldo);
}
