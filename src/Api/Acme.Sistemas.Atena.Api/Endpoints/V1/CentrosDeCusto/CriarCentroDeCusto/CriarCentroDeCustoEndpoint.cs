using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.CriarCentroDeCusto;

public sealed class CriarCentroDeCustoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/centros-de-custo", async (
            CriarCentroDeCustoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/centros-de-custo/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("CentrosDeCusto")
        .WithName("CriarCentroDeCusto")
        .Produces<CriarCentroDeCustoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
