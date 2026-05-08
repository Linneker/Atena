using Acme.Sistemas.Services.V1.Divida.Command.AlterarDivida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.AlterarDivida;

public static class AlterarDividaMap
{
    public static AlterarDividaCommand ToCommand(this AlterarDividaRequest request, Guid id)
        => new(id, request.Credor, request.Descricao, request.ValorOriginal,
            request.TaxaJurosMensal, request.DataInicio, request.DataFim, request.NumeroParcelas);

    public static AlterarDividaResponse ToResponse(this AlterarDividaCommandResult result)
        => new(result.Id);
}
