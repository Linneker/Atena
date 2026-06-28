using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.CriarDespesa;

public sealed class CriarDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/despesas", async (
            CriarDespesaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/despesas/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("CriarDespesa")
        .Produces<CriarDespesaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
