namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.CriarOrcamento;

public sealed record CriarOrcamentoResponse(
    Guid Id,
    string Numero,
    decimal ValorTotal);
