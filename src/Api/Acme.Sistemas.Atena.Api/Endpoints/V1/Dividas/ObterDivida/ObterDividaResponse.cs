using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ObterDivida;

public sealed record ObterDividaResponse(
    Guid Id,
    string Credor,
    string? Descricao,
    decimal ValorOriginal,
    decimal ValorPago,
    decimal Saldo,
    decimal? TaxaJurosMensal,
    DateTime DataInicio,
    DateTime? DataFim,
    int NumeroParcelas,
    StatusConta Status,
    DateTime CreatedAt);
