using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ListarContasReceber;

public sealed record ListarContasReceberResponseItem(
    Guid Id,
    string Descricao,
    Guid? ClienteId,
    string? ClienteNome,
    decimal ValorOriginal,
    decimal ValorRecebido,
    decimal Saldo,
    DateTime DataVencimento,
    StatusConta Status,
    int DiasAtraso);

public sealed record ListarContasReceberResponse(
    IReadOnlyList<ListarContasReceberResponseItem> Items,
    long Total,
    int Skip,
    int Take);
