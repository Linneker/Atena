using Acme.Sistemas.Services.V1.FeatureFlags.Command.RecarregarFeatureFlags;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.RecarregarFeatureFlags;

public static class RecarregarFeatureFlagsMap
{
    public static RecarregarFeatureFlagsCommand ToCommand(this RecarregarFeatureFlagsRequest _)
        => new();

    public static RecarregarFeatureFlagsResponse ToResponse(this RecarregarFeatureFlagsCommandResult result)
        => new(result.RecarregadoEm);
}
