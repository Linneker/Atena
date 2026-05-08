namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.CriarContaReceber;

public sealed record CriarContaReceberResponse(
    Guid Id,
    string Descricao,
    decimal ValorOriginal,
    DateTime DataVencimento);
