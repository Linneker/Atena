using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ReceberReceita;

public sealed record ReceberReceitaResponse(
    Guid Id,
    StatusPagamento StatusRecebimento,
    decimal ValorRecebido,
    DateTime DataRecebimento);
