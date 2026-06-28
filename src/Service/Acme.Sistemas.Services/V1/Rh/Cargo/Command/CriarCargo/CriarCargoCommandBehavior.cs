using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Command.CriarCargo;

public sealed class CriarCargoCommandBehavior
    : IPipelineBehavior<CriarCargoCommand, ResponseDefault<CriarCargoCommandResult>>
{
    public Task<ResponseDefault<CriarCargoCommandResult>> Handle(
        CriarCargoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarCargoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
