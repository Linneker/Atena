using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.FeatureFlags.Command.RecarregarFeatureFlags;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FeatureFlags.RecarregarFeatureFlags;

public sealed class RecarregarFeatureFlagsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/feature-flags/recarregar", async (
            IMediator mediator, CancellationToken ct) =>
        {
            var r = await mediator.Send(new RecarregarFeatureFlagsCommand(), ct);
            return r.IsSuccess ? Results.Ok(r.Content) : Results.Json(r, statusCode: r.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.FeatureFlags, Permissions.Acoes.Editar))
        .WithTags("FeatureFlags")
        .WithName("RecarregarFeatureFlags")
        .Produces<RecarregarFeatureFlagsCommandResult>();
    }
}
