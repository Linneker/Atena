using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.BaixarContaPagar;

public sealed class BaixarContaPagarEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contas-pagar/{id:guid}/baixar", async (
            Guid id,
            BaixarContaPagarRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("ContasPagar")
        .WithName("BaixarContaPagar")
        .Produces<BaixarContaPagarResponse>()
        .ProducesValidationProblem();
    }
}
