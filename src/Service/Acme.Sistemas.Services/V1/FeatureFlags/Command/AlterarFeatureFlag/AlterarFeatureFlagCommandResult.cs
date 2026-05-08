namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

public sealed record AlterarFeatureFlagCommandResult(string Key, object? Value, string Type);
