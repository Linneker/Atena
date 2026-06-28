using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.AlterarCargo;

public sealed class AlterarCargoCommandBehavior
    : IPipelineBehavior<AlterarCargoCommand, ResponseDefault<AlterarCargoCommandResult>>
{
    public Task<ResponseDefault<AlterarCargoCommandResult>> Handle(
        AlterarCargoCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarCargoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
