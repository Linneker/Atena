using Acme.Sistemas.Services.V1.Relatorios.Aging;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Aging.AgingContasPagar;

public static class AgingContasPagarMap
{
    public static AgingQuery ToQuery(this AgingContasPagarRequest _) => new(TipoAging.ContasPagar);

    public static AgingContasPagarResponse ToResponse(this AgingQueryResult result)
        => new(result.Tipo,
            result.Faixas.Select(f => new AgingFaixaResponse(f.Faixa, f.Quantidade, f.Valor)).ToArray(),
            result.TotalGeral, result.QuantidadeGeral);
}
