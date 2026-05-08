using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ReceberContaReceber;

public sealed record ReceberContaReceberResponse(
    Guid Id,
    StatusConta Status,
    decimal ValorRecebido,
    decimal Saldo);
