using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ExcluirDespesa;

public sealed class ExcluirDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/despesas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ExcluirDespesaRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("ExcluirDespesa")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
