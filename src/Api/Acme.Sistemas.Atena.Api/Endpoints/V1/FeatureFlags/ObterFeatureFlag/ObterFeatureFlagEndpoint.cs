using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ObterFeatureFlag;

public sealed class ObterFeatureFlagEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/feature-flags/{key}", async (
            string key,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ObterFeatureFlagRequest(key);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Ler))
        .WithTags("FeatureFlags")
        .WithName("ObterFeatureFlag")
        .Produces<ObterFeatureFlagResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
