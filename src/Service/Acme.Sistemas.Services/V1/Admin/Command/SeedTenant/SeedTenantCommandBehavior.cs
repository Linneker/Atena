using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;

/// <summary>Behavior do SeedTenantCommand. No-op pass-through — convenção do blueprint.</summary>
public sealed class SeedTenantCommandBehavior
    : IPipelineBehavior<SeedTenantCommand, ResponseDefault<SeedTenantCommandResult>>
{
    public Task<ResponseDefault<SeedTenantCommandResult>> Handle(
        SeedTenantCommand request,
        RequestHandlerDelegate<ResponseDefault<SeedTenantCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
