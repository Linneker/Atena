using Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ListarFeatureFlags;

public static class ListarFeatureFlagsMap
{
    public static ListarFeatureFlagsQuery ToQuery(this ListarFeatureFlagsRequest _)
        => new();

    public static ListarFeatureFlagsResponse ToResponse(this ListarFeatureFlagsQueryResult result)
        => new(result.Items.Select(i => new ListarFeatureFlagsResponseItem(i.Key, i.Value, i.Type)).ToArray());
}
