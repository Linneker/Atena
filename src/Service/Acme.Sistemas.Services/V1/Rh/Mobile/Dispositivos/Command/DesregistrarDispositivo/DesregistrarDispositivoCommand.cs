using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Command.DesregistrarDispositivo;

public sealed record DesregistrarDispositivoCommand(string DeviceId)
    : IRequest<ResponseDefault<DesregistrarDispositivoCommandResult>>;
