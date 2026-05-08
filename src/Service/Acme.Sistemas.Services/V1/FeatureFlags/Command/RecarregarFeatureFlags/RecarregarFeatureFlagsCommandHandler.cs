using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.RecarregarFeatureFlags;

public sealed class RecarregarFeatureFlagsCommandHandler
    : IRequestHandler<RecarregarFeatureFlagsCommand, ResponseDefault<RecarregarFeatureFlagsCommandResult>>
{
    private readonly IFeatureFlagService _service;

    public RecarregarFeatureFlagsCommandHandler(IFeatureFlagService service) => _service = service;

    public async Task<ResponseDefault<RecarregarFeatureFlagsCommandResult>> Handle(
        RecarregarFeatureFlagsCommand request, CancellationToken cancellationToken)
    {
        var ts = await _service.ReloadAsync(cancellationToken);
        return ResponseDefault<RecarregarFeatureFlagsCommandResult>.Ok(
            new RecarregarFeatureFlagsCommandResult(ts));
    }
}
