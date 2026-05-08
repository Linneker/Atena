using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.CriarContaReceber;

public sealed class CriarContaReceberEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/contas-receber", async (
            CriarContaReceberRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/contas-receber/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("ContasReceber")
        .WithName("CriarContaReceber")
        .Produces<CriarContaReceberResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
