using System.Text.Json;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.AlterarFeatureFlag;

public sealed record AlterarFeatureFlagRequest(JsonElement Value);
