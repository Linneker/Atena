using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

public sealed record ListarFeatureFlagsQuery() : IRequest<ResponseDefault<ListarFeatureFlagsQueryResult>>;
