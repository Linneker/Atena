using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.RecarregarFeatureFlags;

public sealed class RecarregarFeatureFlagsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/feature-flags/recarregar", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new RecarregarFeatureFlagsRequest();
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Editar))
        .WithTags("FeatureFlags")
        .WithName("RecarregarFeatureFlags")
        .Produces<RecarregarFeatureFlagsResponse>();
    }
}
