using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.CriarJornada;

public sealed record CriarJornadaCommand(
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    string JanelasJson,
    bool PermiteMarcarIntervalo = true,
    int ToleranciaMinutos = 10) : IRequest<ResponseDefault<CriarJornadaCommandResult>>;
