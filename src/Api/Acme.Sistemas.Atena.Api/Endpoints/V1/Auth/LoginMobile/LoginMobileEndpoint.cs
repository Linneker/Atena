using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.LoginMobile;

public sealed class LoginMobileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/autenticacao/login-mobile", async (
            LoginMobileRequest request,
            IMediator mediator,
            HttpContext http,
            CancellationToken cancellationToken) =>
        {
            var ip = http.Connection.RemoteIpAddress?.ToString();
            var ua = http.Request.Headers.UserAgent.ToString();

            var result = await mediator.Send(request.ToCommand(ip, ua), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            http.Response.Headers.CacheControl = "no-store";
            return Results.Ok(result.Content.ToResponse());
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithName("LoginMobile")
        .Produces<LoginMobileResponse>()
        .ProducesValidationProblem();
    }
}
