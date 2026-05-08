using Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ObterDivida;

public static class ObterDividaMap
{
    public static ObterDividaQuery ToQuery(this ObterDividaRequest request)
        => new(request.Id);

    public static ObterDividaResponse ToResponse(this ObterDividaQueryResult result)
        => new(result.Id, result.Credor, result.Descricao, result.ValorOriginal, result.ValorPago,
            result.Saldo, result.TaxaJurosMensal, result.DataInicio, result.DataFim,
            result.NumeroParcelas, result.Status, result.CreatedAt);
}
