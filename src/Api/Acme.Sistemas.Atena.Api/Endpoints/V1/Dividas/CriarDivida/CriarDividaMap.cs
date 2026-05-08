using Acme.Sistemas.Services.V1.Divida.Command.CriarDivida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.CriarDivida;

public static class CriarDividaMap
{
    public static CriarDividaCommand ToCommand(this CriarDividaRequest request)
        => new(request.Credor, request.Descricao, request.ValorOriginal, request.TaxaJurosMensal,
            request.DataInicio, request.DataFim, request.NumeroParcelas);

    public static CriarDividaResponse ToResponse(this CriarDividaCommandResult result)
        => new(result.Id, result.Credor, result.ValorOriginal);
}
