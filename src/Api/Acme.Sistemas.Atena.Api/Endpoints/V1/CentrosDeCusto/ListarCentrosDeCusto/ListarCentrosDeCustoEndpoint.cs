using Acme.Sistemas.Core.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.ListarCentrosDeCusto;

public sealed class ListarCentrosDeCustoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/centros-de-custo", async (
            [AsParameters] ListarCentrosDeCustoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("CentrosDeCusto")
        .WithName("ListarCentrosDeCusto")
        .Produces<ListarCentrosDeCustoResponse>();
    }
}
