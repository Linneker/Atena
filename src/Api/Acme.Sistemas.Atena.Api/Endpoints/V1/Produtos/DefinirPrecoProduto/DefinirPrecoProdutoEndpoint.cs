using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.DefinirPrecoProduto;

public sealed class DefinirPrecoProdutoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/produtos/{id:guid}/precos", async (
            Guid id,
            DefinirPrecoProdutoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/produtos/{id}/precos/{response.PrecoId}", response);
        })
        .RequireAuthorization()
        .WithTags("Produtos")
        .WithName("DefinirPrecoProduto")
        .Produces<DefinirPrecoProdutoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
