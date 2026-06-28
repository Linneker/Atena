using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.RenovarToken;

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

            var result = await mediator.Send(request.ToCommand(userAgent, ip), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithName("RenovarToken")
        .Produces<RenovarTokenResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
