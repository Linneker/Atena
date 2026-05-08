namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

public sealed record ObterFeatureFlagQueryResult(string Key, object? Value, string Type);
