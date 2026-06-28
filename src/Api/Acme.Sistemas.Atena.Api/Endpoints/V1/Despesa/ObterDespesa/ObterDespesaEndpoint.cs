using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ObterDespesa;

public sealed class ObterDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/despesas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ObterDespesaRequest(id);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("ObterDespesa")
        .Produces<ObterDespesaResponse>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
