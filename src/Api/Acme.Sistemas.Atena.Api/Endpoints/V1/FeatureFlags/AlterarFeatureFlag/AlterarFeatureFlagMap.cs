using Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.AlterarFeatureFlag;

public static class AlterarFeatureFlagMap
{
    public static AlterarFeatureFlagCommand ToCommand(this AlterarFeatureFlagRequest request, string key)
        => new(key, request.Value);

    public static AlterarFeatureFlagResponse ToResponse(this AlterarFeatureFlagCommandResult result)
        => new(result.Key, result.Value, result.Type);
}
