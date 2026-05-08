using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.CriarContaPagar;

public sealed class CriarContaPagarEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contas-pagar", async (
            CriarContaPagarRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/contas-pagar/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("ContasPagar")
        .WithName("CriarContaPagar")
        .Produces<CriarContaPagarResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
