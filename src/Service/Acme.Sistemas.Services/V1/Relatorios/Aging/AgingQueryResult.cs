using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Aging;

public sealed record AgingFaixa(string Faixa, int Quantidade, decimal Valor);

public sealed record AgingQueryResult(
    TipoAging Tipo,
    IReadOnlyList<AgingFaixa> Faixas,
    decimal TotalGeral,
    int QuantidadeGeral);
