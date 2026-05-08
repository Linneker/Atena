using Acme.Sistemas.Services.V1.ContaPagar.Command.BaixarContaPagar;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.BaixarContaPagar;

public static class BaixarContaPagarMap
{
    public static BaixarContaPagarCommand ToCommand(this BaixarContaPagarRequest request, Guid id)
        => new(id, request.ValorPago, request.DataPagamento, request.FormaPagamento, request.Observacao);

    public static BaixarContaPagarResponse ToResponse(this BaixarContaPagarCommandResult result)
        => new(result.Id, result.Status, result.ValorPago, result.Saldo);
}
