using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

public sealed record ObterFeatureFlagQuery(string Key) : IRequest<ResponseDefault<ObterFeatureFlagQueryResult>>;
