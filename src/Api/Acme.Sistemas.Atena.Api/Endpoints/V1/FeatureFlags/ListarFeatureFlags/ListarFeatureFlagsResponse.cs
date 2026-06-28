namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ListarFeatureFlags;

public sealed record ListarFeatureFlagsResponseItem(
    string Key,
    object? Value,
    string Type);

public sealed record ListarFeatureFlagsResponse(
    IReadOnlyList<ListarFeatureFlagsResponseItem> Items);
