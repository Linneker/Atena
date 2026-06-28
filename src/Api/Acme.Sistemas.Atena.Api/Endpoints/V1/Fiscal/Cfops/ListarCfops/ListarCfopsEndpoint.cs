using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.Cfops.ListarCfops;

public sealed class ListarCfopsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/fiscal/cfops", async (
            string? categoria,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarCfopsRequest(categoria);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Fiscal")
        .WithName("ListarCfops")
        .Produces<ListarCfopsResponse>();
    }
}
