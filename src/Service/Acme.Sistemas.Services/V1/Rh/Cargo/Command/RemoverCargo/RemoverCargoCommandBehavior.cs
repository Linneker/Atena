using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.RemoverCargo;

public sealed class RemoverCargoCommandBehavior
    : IPipelineBehavior<RemoverCargoCommand, ResponseDefault<RemoverCargoCommandResult>>
{
    public Task<ResponseDefault<RemoverCargoCommandResult>> Handle(
        RemoverCargoCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverCargoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
