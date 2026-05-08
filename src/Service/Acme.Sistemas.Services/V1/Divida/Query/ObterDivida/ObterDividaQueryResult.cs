using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Divida.Query.ObterDivida;

public sealed record ObterDividaQueryResult(
    Guid Id, string Credor, string? Descricao,
    decimal ValorOriginal, decimal ValorPago, decimal Saldo,
    decimal? TaxaJurosMensal, DateTime DataInicio, DateTime? DataFim,
    int NumeroParcelas, StatusConta Status, DateTime CreatedAt);
