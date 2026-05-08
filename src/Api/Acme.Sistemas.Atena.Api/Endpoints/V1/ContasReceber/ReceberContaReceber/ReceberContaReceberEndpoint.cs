using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ReceberContaReceber;

public sealed class ReceberContaReceberEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contas-receber/{id:guid}/receber", async (
            Guid id,
            ReceberContaReceberRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("ContasReceber")
        .WithName("ReceberContaReceber")
        .Produces<ReceberContaReceberResponse>()
        .ProducesValidationProblem();
    }
}
