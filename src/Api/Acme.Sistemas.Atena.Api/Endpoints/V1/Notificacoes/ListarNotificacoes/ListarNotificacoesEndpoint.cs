using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Notificacoes.ListarNotificacoes;

public sealed class ListarNotificacoesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/notificacoes", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new ListarNotificacoesRequest().ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Notificacoes")
        .WithName("ListarNotificacoes")
        .Produces<IReadOnlyList<NotificacaoItemResponse>>()
        .ProducesProblem(StatusCodes.Status401Unauthorized);
    }
}
