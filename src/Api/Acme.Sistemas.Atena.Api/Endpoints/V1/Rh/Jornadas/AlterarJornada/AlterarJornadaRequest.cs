using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Jornadas.AlterarJornada;

public sealed record AlterarJornadaRequest(
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    string JanelasJson,
    bool PermiteMarcarIntervalo,
    int ToleranciaMinutos,
    bool Ativo);
