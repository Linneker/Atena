using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ListarDividas;

public sealed record ListarDividasResponseItem(
    Guid Id,
    string Credor,
    decimal ValorOriginal,
    decimal ValorPago,
    decimal Saldo,
    DateTime DataInicio,
    DateTime? DataFim,
    int NumeroParcelas,
    StatusConta Status);

public sealed record ListarDividasResponse(IReadOnlyList<ListarDividasResponseItem> Items, long Total);
