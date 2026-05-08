using Acme.Sistemas.Services.V1.Relatorios.Aging;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Aging.AgingContasReceber;

public static class AgingContasReceberMap
{
    public static AgingQuery ToQuery(this AgingContasReceberRequest _) => new(TipoAging.ContasReceber);

    public static AgingContasReceberResponse ToResponse(this AgingQueryResult result)
        => new(result.Tipo,
            result.Faixas.Select(f => new AgingFaixaResponse(f.Faixa, f.Quantidade, f.Valor)).ToArray(),
            result.TotalGeral, result.QuantidadeGeral);
}
