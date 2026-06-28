using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.ObterJornada;

public sealed record ObterJornadaResponse(
    Guid Id,
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    string JanelasJson,
    bool PermiteMarcarIntervalo,
    int ToleranciaMinutos,
    bool Ativo);
