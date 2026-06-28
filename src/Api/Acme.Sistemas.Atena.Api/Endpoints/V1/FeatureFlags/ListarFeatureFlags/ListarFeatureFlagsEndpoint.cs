using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ListarFeatureFlags;

public sealed class ListarFeatureFlagsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/feature-flags", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarFeatureFlagsRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Ler))
        .WithTags("FeatureFlags")
        .WithName("ListarFeatureFlags")
        .Produces<ListarFeatureFlagsResponse>();
    }
}
