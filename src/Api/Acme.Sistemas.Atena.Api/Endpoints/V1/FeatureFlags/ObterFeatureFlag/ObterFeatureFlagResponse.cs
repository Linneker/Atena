namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ObterFeatureFlag;

public sealed record ObterFeatureFlagResponse(
    string Key,
    object? Value,
    string Type);
