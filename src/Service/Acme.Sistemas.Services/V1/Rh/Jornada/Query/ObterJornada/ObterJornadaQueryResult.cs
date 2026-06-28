using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;

public sealed record ObterJornadaQueryResult(
    Guid Id,
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    string JanelasJson,
    bool PermiteMarcarIntervalo,
    int ToleranciaMinutos,
    bool Ativo);
