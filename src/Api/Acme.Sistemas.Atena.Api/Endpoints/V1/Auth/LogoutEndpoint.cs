using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Autenticacao.Command.Logout;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth;

public sealed record LogoutRequest(string RefreshToken);

public sealed class LogoutEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/logout", async (
            LogoutRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new LogoutCommand(request.RefreshToken), cancellationToken);
            return Results.NoContent();
        })
        .RequireAuthorization()
        .WithTags("Auth")
        .WithName("Logout")
        .Produces(StatusCodes.Status204NoContent);
    }
}
