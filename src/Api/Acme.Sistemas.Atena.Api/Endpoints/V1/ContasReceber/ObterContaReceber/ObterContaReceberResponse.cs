using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ObterContaReceber;

public sealed record ObterContaReceberResponse(
    Guid Id,
    string Descricao,
    Guid? ClienteId,
    string? ClienteNome,
    Guid? ReceitaId,
    Guid? PlanoDeContasId,
    decimal ValorOriginal,
    decimal ValorRecebido,
    decimal Saldo,
    DateTime DataVencimento,
    DateTime? DataRecebimento,
    StatusConta Status,
    int DiasAtraso,
    string? ObservacaoRecebimento,
    DateTime CreatedAt);
