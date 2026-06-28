using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Command.RemoverJornada;

public sealed record RemoverJornadaCommand(Guid Id)
    : IRequest<ResponseDefault<RemoverJornadaCommandResult>>;
