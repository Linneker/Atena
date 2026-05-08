using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

public sealed class ListarFeatureFlagsQueryHandler
    : IRequestHandler<ListarFeatureFlagsQuery, ResponseDefault<ListarFeatureFlagsQueryResult>>
{
    private readonly IFeatureFlagService _service;

    public ListarFeatureFlagsQueryHandler(IFeatureFlagService service) => _service = service;

    public Task<ResponseDefault<ListarFeatureFlagsQueryResult>> Handle(
        ListarFeatureFlagsQuery request, CancellationToken cancellationToken)
    {
        var items = _service.ListAll()
            .Select(f => new ListarFeatureFlagsQueryItem(f.Key, f.Value, f.Type.ToString()))
            .ToList();
        return Task.FromResult(ResponseDefault<ListarFeatureFlagsQueryResult>.Ok(
            new ListarFeatureFlagsQueryResult(items)));
    }
}
