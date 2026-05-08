using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.ObterFeatureFlag;

public sealed class ObterFeatureFlagEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/feature-flags/{key}", async (
            string key, IMediator mediator, CancellationToken ct) =>
        {
            var r = await mediator.Send(new ObterFeatureFlagQuery(key), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Ler))
        .WithTags("FeatureFlags")
        .WithName("ObterFeatureFlag")
        .Produces<ObterFeatureFlagQueryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
