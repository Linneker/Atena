using Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ObterFeatureFlag;

public static class ObterFeatureFlagMap
{
    public static ObterFeatureFlagQuery ToQuery(this ObterFeatureFlagRequest request)
        => new(request.Key);

    public static ObterFeatureFlagResponse ToResponse(this ObterFeatureFlagQueryResult result)
        => new(result.Key, result.Value, result.Type);
}
