using Acme.Sistemas.Core.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ListarEstoques;

public sealed class ListarEstoquesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/estoques", async (
            [AsParameters] ListarEstoquesRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Estoque")
        .WithName("ListarEstoques")
        .Produces<ListarEstoquesResponse>();
    }
}
