using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Autenticacao.Command.ConfirmarEmail;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.ConfirmarEmail;
public sealed record ConfirmarEmailRequest(string Token);

public sealed class ConfirmarEmailEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/auth/confirmar-email", async (
            ConfirmarEmailRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ConfirmarEmailCommand(request.Token), cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .AllowAnonymous()
        .RequireRateLimiting("email-confirmation")
        .WithTags("Auth")
        .WithName("ConfirmarEmail")
        .Produces<ConfirmarEmailCommandResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
