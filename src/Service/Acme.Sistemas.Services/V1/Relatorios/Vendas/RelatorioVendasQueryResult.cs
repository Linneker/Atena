using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Vendas;

public sealed record RelatorioVendasLinha(
    Guid Id, decimal? Quantidade, decimal Total, int? Faturamentos);

public sealed record RelatorioVendasResult(
    DateTime Inicio, DateTime Fim, AgrupamentoVendas Agrupamento,
    decimal TotalGeral, IReadOnlyList<RelatorioVendasLinha> Linhas);
