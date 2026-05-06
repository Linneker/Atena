using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Despesa.Command.ExcluirDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa;

public sealed class ExcluirDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/despesas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ExcluirDespesaCommand(id), cancellationToken);
            return response.IsSuccess
                ? Results.NoContent()
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("ExcluirDespesa")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
