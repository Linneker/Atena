using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarSaida;

public sealed class RegistrarSaidaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/estoque/saidas", async (
            RegistrarSaidaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/estoque/movimentos/{response.MovimentoId}", response);
        })
        .RequireAuthorization()
        .WithTags("Estoque")
        .WithName("RegistrarSaida")
        .Produces<RegistrarSaidaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
