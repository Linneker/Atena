using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.CriarReceita;

public sealed class CriarReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/receitas", async (
            CriarReceitaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/receitas/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("CriarReceita")
        .Produces<CriarReceitaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
