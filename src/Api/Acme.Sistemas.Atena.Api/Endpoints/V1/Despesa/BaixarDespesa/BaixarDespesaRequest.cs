using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.BaixarDespesa;

public sealed record BaixarDespesaRequest(
    decimal ValorPago,
    DateTime DataPagamento,
    FormaPagamento FormaPagamento,
    string? Observacao);
