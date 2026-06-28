using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarEntrada;

public sealed class RegistrarEntradaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/estoque/entradas", async (
            RegistrarEntradaRequest request,
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
        .WithName("RegistrarEntrada")
        .Produces<RegistrarEntradaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
