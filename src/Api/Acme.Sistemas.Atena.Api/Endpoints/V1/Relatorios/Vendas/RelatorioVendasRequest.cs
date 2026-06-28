using Acme.Sistemas.Services.V1.Relatorios.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.Vendas;

public sealed record RelatorioVendasRequest(
    DateTime Inicio,
    DateTime Fim,
    AgrupamentoVendas Agrupamento);
