using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.ContaPagar.Query.ListarContasPagar;

public sealed record ListarContasPagarQueryItem(
    Guid Id, string Descricao, Guid? FornecedorId,
    decimal ValorOriginal, decimal ValorPago, decimal Saldo,
    DateTime DataVencimento, StatusConta Status,
    bool Vencida, int DiasParaVencer);

public sealed record ListarContasPagarQueryResult(
    IReadOnlyList<ListarContasPagarQueryItem> Items, long Total, int Skip, int Take);
