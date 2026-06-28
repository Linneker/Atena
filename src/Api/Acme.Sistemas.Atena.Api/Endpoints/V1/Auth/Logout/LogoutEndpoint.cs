using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.Logout;

public sealed class LogoutEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/logout", async (
            LogoutRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            await mediator.Send(request.ToCommand(), cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithTags("Auth")
        .WithName("Logout")
        .Produces(StatusCodes.Status204NoContent);
    }
}
