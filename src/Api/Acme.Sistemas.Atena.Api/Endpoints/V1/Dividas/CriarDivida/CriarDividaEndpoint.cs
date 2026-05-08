using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.CriarDivida;

public sealed class CriarDividaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/dividas", async (
            CriarDividaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/dividas/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Dividas")
        .WithName("CriarDivida")
        .Produces<CriarDividaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
