using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.AlterarReceita;

public sealed class AlterarReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/receitas/{id:guid}", async (
            Guid id,
            AlterarReceitaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("AlterarReceita")
        .Produces<AlterarReceitaResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
