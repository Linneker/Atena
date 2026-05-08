using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposValorProduto.ListarTiposValorProduto;

public sealed class ListarTiposValorProdutoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tipos-valor-produto", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarTiposValorProdutoRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("TiposValorProduto")
        .WithName("ListarTiposValorProduto")
        .Produces<ListarTiposValorProdutoResponse>();
    }
}
