using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.ListarJornadas;

public sealed record ListarJornadasResponseItem(
    Guid Id, string Nome, TipoJornada Tipo,
    decimal CargaSemanalHoras, decimal? CargaDiariaHoras,
    int ToleranciaMinutos, bool Ativo);

public sealed record ListarJornadasResponse(
    IReadOnlyList<ListarJornadasResponseItem> Items, long Total);
