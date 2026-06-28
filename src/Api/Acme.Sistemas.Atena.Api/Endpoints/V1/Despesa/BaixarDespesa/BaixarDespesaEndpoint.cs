using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.BaixarDespesa;

public sealed class BaixarDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/despesas/{id:guid}/baixar", async (
            Guid id,
            BaixarDespesaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("BaixarDespesa")
        .Produces<BaixarDespesaResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
