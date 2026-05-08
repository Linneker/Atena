using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.ObterContaPagar;

public sealed record ObterContaPagarResponse(
    Guid Id,
    string Descricao,
    Guid? FornecedorId,
    Guid? DespesaId,
    Guid? PlanoDeContasId,
    decimal ValorOriginal,
    decimal ValorPago,
    decimal Saldo,
    DateTime DataVencimento,
    DateTime? DataPagamento,
    StatusConta Status,
    string? Observacao,
    DateTime CreatedAt);
