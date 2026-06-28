using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;

public sealed record ListarJornadasQueryItem(
    Guid Id,
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    int ToleranciaMinutos,
    bool Ativo);

public sealed record ListarJornadasQueryResult(
    IReadOnlyList<ListarJornadasQueryItem> Items,
    long Total);
