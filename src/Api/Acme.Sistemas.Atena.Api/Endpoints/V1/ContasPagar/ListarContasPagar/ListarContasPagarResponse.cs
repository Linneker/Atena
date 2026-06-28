using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.ListarContasPagar;

public sealed record ListarContasPagarResponseItem(
    Guid Id,
    string Descricao,
    Guid? FornecedorId,
    string? FornecedorNome,
    decimal ValorOriginal,
    decimal ValorPago,
    decimal Saldo,
    DateTime DataVencimento,
    StatusConta Status,
    bool Vencida,
    int DiasParaVencer);

public sealed record ListarContasPagarResponse(
    IReadOnlyList<ListarContasPagarResponseItem> Items,
    long Total,
    int Skip,
    int Take);
