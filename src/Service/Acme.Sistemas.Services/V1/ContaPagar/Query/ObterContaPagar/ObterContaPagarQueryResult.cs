using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ObterContaPagar;

public sealed record ObterContaPagarQueryResult(
    Guid Id, string Descricao,
    Guid? FornecedorId, string? FornecedorNome,
    Guid? DespesaId, Guid? PlanoDeContasId,
    decimal ValorOriginal, decimal ValorPago, decimal Saldo,
    DateTime DataVencimento, DateTime? DataPagamento, StatusConta Status,
    string? Observacao, DateTime CreatedAt);
