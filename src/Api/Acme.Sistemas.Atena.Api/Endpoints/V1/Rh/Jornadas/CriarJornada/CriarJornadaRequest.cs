using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.CriarJornada;

public sealed record CriarJornadaRequest(
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    string JanelasJson,
    bool PermiteMarcarIntervalo = true,
    int ToleranciaMinutos = 10);
