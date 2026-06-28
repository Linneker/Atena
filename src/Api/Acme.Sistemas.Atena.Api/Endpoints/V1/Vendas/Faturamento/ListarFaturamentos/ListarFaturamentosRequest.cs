namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.ListarFaturamentos;

public sealed record ListarFaturamentosRequest(
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 50);
