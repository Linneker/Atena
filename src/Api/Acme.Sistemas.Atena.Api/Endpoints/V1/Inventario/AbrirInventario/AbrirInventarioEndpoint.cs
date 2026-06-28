using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.AbrirInventario;

public sealed class AbrirInventarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/inventarios", async (
            AbrirInventarioRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/inventarios/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Inventarios")
        .WithName("AbrirInventario")
        .Produces<AbrirInventarioResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
