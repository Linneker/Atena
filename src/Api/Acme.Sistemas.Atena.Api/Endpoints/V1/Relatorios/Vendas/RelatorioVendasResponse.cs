using Acme.Sistemas.Services.V1.Relatorios.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Vendas;

public sealed record RelatorioVendasResponseLinha(
    Guid Id,
    decimal? Quantidade,
    decimal Total,
    int? Faturamentos);

public sealed record RelatorioVendasResponse(
    DateTime Inicio,
    DateTime Fim,
    AgrupamentoVendas Agrupamento,
    decimal TotalGeral,
    IReadOnlyList<RelatorioVendasResponseLinha> Linhas);
