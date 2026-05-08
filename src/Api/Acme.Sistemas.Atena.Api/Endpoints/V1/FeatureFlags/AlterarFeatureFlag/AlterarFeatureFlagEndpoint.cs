using System.Text.Json;
using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.AlterarFeatureFlag;

public sealed record AlterarFeatureFlagRequest(JsonElement Value);

public sealed class AlterarFeatureFlagEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/feature-flags/{key}", async (
            string key, AlterarFeatureFlagRequest body, IMediator mediator, CancellationToken ct) =>
        {
            var r = await mediator.Send(new AlterarFeatureFlagCommand(key, body.Value), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Editar))
        .WithTags("FeatureFlags")
        .WithName("AlterarFeatureFlag")
        .Produces<AlterarFeatureFlagCommandResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
