using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Fiscal.Csts.ListarCsts;

public sealed class ListarCstsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/fiscal/csts/{tipo}", async (
            string tipo,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarCstsRequest(tipo);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Fiscal")
        .WithName("ListarCsts")
        .Produces<ListarCstsResponse>();
    }
}
