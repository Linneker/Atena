using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.RecarregarFeatureFlags;

public sealed record RecarregarFeatureFlagsCommand() : IRequest<ResponseDefault<RecarregarFeatureFlagsCommandResult>>;
