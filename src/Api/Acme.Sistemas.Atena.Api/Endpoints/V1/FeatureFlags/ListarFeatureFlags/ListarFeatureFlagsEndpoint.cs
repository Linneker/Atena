using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ListarFeatureFlags;

public sealed class ListarFeatureFlagsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/feature-flags", async (
            IMediator mediator, CancellationToken ct) =>
        {
            var r = await mediator.Send(new ListarFeatureFlagsQuery(), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Ler))
        .WithTags("FeatureFlags")
        .WithName("ListarFeatureFlags")
        .Produces<ListarFeatureFlagsQueryResult>();
    }
}
