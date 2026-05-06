using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Receita.Command.ExcluirReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita;

public sealed class ExcluirReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/receitas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ExcluirReceitaCommand(id), cancellationToken);
            return response.IsSuccess
                ? Results.NoContent()
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("ExcluirReceita")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
