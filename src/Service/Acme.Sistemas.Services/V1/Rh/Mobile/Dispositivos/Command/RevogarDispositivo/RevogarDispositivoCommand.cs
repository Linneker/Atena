using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.RevogarDispositivo;

/// <summary>Revoga dispositivo (uso admin) — qualquer device do tenant pelo ID.</summary>
public sealed record RevogarDispositivoCommand(Guid Id)
    : IRequest<ResponseDefault<RevogarDispositivoCommandResult>>;
