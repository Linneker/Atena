using Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.BaixarDespesa;

public static class BaixarDespesaMap
{
    public static BaixarDespesaCommand ToCommand(this BaixarDespesaRequest request, Guid id)
        => new(id, request.ValorPago, request.DataPagamento, request.FormaPagamento, request.Observacao);

    public static BaixarDespesaResponse ToResponse(this BaixarDespesaCommandResult result)
        => new(result.Id, result.StatusPagamento, result.ValorPago, result.DataPagamento);
}
