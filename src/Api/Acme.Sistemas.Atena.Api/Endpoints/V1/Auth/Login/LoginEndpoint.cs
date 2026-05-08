using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Autenticacao.Command.Login;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.Login;
public sealed record LoginRequest(string Cnpj, string Email, string Senha);

public sealed class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/login", async (
            LoginRequest request,
            HttpContext httpContext,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var userAgent = httpContext.Request.Headers.UserAgent.ToString();
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();
            var command = new LoginCommand(request.Cnpj, request.Email, request.Senha, userAgent, ip);

            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .AllowAnonymous()
        .RequireRateLimiting("auth-login")
        .WithTags("Auth")
        .WithName("Login")
        .Produces<LoginCommandResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
