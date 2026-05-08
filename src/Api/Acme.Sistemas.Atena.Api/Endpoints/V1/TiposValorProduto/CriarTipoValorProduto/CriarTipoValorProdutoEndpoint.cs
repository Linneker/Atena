using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposValorProduto.CriarTipoValorProduto;

public sealed class CriarTipoValorProdutoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tipos-valor-produto", async (
            CriarTipoValorProdutoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/tipos-valor-produto/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("TiposValorProduto")
        .WithName("CriarTipoValorProduto")
        .Produces<CriarTipoValorProdutoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
