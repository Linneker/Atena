using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Notificacoes.MarcarNotificacaoLida;

public sealed class MarcarNotificacaoLidaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/notificacoes/{id:guid}/ler", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new MarcarNotificacaoLidaRequest(id).ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Notificacoes")
        .WithName("MarcarNotificacaoLida")
        .Produces<MarcarNotificacaoLidaResponse>()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
