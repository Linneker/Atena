using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.AlterarDespesa;

public sealed class AlterarDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/despesas/{id:guid}", async (
            Guid id,
            AlterarDespesaRequest request,
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
        .WithName("AlterarDespesa")
        .Produces<AlterarDespesaResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
