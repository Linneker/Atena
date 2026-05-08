using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Despesa.Query.ObterDespesa;

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
            var response = await mediator.Send(new ObterDespesaQuery(id), cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("ObterDespesa")
        .Produces<ObterDespesaQueryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
