using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ExcluirReceita;

public sealed class ExcluirReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/receitas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ExcluirReceitaRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("ExcluirReceita")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
