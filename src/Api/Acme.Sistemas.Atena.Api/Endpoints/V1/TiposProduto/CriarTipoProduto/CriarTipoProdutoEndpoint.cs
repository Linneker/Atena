using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposProduto.CriarTipoProduto;

public sealed class CriarTipoProdutoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/tipos-produto", async (
            CriarTipoProdutoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/tipos-produto/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("TiposProduto")
        .WithName("CriarTipoProduto")
        .Produces<CriarTipoProdutoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
