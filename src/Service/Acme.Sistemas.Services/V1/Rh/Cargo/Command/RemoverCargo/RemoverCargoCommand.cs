using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.RemoverCargo;

public sealed record RemoverCargoCommand(Guid Id)
    : IRequest<ResponseDefault<RemoverCargoCommandResult>>;
