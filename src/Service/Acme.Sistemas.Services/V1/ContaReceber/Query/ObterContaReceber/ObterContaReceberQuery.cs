using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ObterContaReceber;

public sealed record ObterContaReceberQuery(Guid Id) : IRequest<ResponseDefault<ObterContaReceberQueryResult>>;

public sealed record ObterContaReceberQueryResult(
    Guid Id, string Descricao, Guid? ClienteId, Guid? ReceitaId, Guid? PlanoDeContasId,
    decimal ValorOriginal, decimal ValorRecebido, decimal Saldo,
    DateTime DataVencimento, DateTime? DataRecebimento, StatusConta Status,
    int DiasAtraso, string? ObservacaoRecebimento, DateTime CreatedAt);
