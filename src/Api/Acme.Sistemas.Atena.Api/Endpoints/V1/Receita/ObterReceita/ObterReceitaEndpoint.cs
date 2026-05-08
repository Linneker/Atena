using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ObterReceita;
public sealed class ObterReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/receitas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ObterReceitaQuery(id), cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("ObterReceita")
        .Produces<ObterReceitaQueryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
