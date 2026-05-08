using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.BaixarContaPagar;

public sealed record BaixarContaPagarRequest(
    decimal ValorPago,
    DateTime DataPagamento,
    FormaPagamento FormaPagamento,
    string? Observacao);
