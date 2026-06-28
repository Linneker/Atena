using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Command.SeedCbos;

public sealed class SeedCbosCommandBehavior
    : IPipelineBehavior<SeedCbosCommand, ResponseDefault<SeedCbosCommandResult>>
{
    public Task<ResponseDefault<SeedCbosCommandResult>> Handle(
        SeedCbosCommand request,
        RequestHandlerDelegate<ResponseDefault<SeedCbosCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
