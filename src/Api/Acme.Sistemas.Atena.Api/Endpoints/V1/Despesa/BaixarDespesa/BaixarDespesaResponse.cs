using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.BaixarDespesa;

public sealed record BaixarDespesaResponse(
    Guid Id,
    StatusPagamento StatusPagamento,
    decimal ValorPago,
    DateTime DataPagamento);
