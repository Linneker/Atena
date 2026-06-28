using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

public sealed record ListarMarcacoesPorPeriodoQueryItem(
    Guid Id,
    DateTime DataHora,
    TipoMarcacao Tipo,
    OrigemMarcacao Origem,
    StatusMarcacao Status,
    string HashIntegridade);

public sealed record ListarMarcacoesPorPeriodoQueryResult(
    IReadOnlyList<ListarMarcacoesPorPeriodoQueryItem> Items,
    int Total);
