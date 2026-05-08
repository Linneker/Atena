namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ReceberContaReceber;

public sealed record ReceberContaReceberRequest(
    decimal ValorRecebido,
    DateTime DataRecebimento,
    string? Observacao);
