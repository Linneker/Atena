using Acme.Sistemas.Services.V1.Relatorios.Aging;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Aging.AgingContasPagar;

public sealed record AgingFaixaResponse(string Faixa, int Quantidade, decimal Valor);

public sealed record AgingContasPagarResponse(
    TipoAging Tipo,
    IReadOnlyList<AgingFaixaResponse> Faixas,
    decimal TotalGeral,
    int QuantidadeGeral);
