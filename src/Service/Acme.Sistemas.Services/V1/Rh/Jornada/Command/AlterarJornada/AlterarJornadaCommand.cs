using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.AlterarJornada;

public sealed record AlterarJornadaCommand(
    Guid Id,
    string Nome,
    TipoJornada Tipo,
    decimal CargaSemanalHoras,
    decimal? CargaDiariaHoras,
    string JanelasJson,
    bool PermiteMarcarIntervalo,
    int ToleranciaMinutos,
    bool Ativo) : IRequest<ResponseDefault<AlterarJornadaCommandResult>>;
