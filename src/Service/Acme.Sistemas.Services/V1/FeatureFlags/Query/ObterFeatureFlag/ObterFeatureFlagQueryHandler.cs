using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

public sealed class ObterFeatureFlagQueryHandler
    : IRequestHandler<ObterFeatureFlagQuery, ResponseDefault<ObterFeatureFlagQueryResult>>
{
    private readonly IFeatureFlagService _service;

    public ObterFeatureFlagQueryHandler(IFeatureFlagService service) => _service = service;

    public Task<ResponseDefault<ObterFeatureFlagQueryResult>> Handle(
        ObterFeatureFlagQuery request, CancellationToken cancellationToken)
    {
        var item = _service.Get(request.Key);
        if (item is null)
            return Task.FromResult(ResponseDefault<ObterFeatureFlagQueryResult>.NotFound(
                $"Feature flag '{request.Key}' não encontrada."));

        return Task.FromResult(ResponseDefault<ObterFeatureFlagQueryResult>.Ok(
            new ObterFeatureFlagQueryResult(item.Key, item.Value, item.Type.ToString())));
    }
}
