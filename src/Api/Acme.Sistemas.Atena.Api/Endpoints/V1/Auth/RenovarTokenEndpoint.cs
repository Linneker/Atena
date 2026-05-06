using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Autenticacao.Command.RenovarToken;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth;

public sealed record RenovarTokenRequest(string RefreshToken);

public sealed class RenovarTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/refresh", async (
            RenovarTokenRequest request,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userAgent = httpContext.Request.Headers.UserAgent.ToString();
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();
            var command = new RenovarTokenCommand(request.RefreshToken, userAgent, ip);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithName("RenovarToken")
        .Produces<RenovarTokenCommandResult>()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
