using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.BaixarContaPagar;

public sealed record BaixarContaPagarResponse(
    Guid Id,
    StatusConta Status,
    decimal ValorPago,
    decimal Saldo);
