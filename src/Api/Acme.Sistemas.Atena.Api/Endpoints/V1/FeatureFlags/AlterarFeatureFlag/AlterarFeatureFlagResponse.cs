namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.AlterarFeatureFlag;

public sealed record AlterarFeatureFlagResponse(
    string Key,
    object? Value,
    string Type);
