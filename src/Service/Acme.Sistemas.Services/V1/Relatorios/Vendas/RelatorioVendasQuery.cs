using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Vendas;

public enum AgrupamentoVendas
{
    Vendedor = 1,
    Cliente = 2,
    Produto = 3
}

public sealed record RelatorioVendasQuery(
    DateTime Inicio,
    DateTime Fim,
    AgrupamentoVendas Agrupamento) : IRequest<ResponseDefault<RelatorioVendasResult>>;

