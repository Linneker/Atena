namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

public sealed record ListarFeatureFlagsQueryItem(string Key, object? Value, string Type);

public sealed record ListarFeatureFlagsQueryResult(IReadOnlyList<ListarFeatureFlagsQueryItem> Items);
