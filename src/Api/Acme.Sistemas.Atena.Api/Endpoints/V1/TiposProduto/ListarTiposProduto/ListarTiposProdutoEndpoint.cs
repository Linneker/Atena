using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.TiposProduto.ListarTiposProduto;

public sealed class ListarTiposProdutoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/tipos-produto", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ListarTiposProdutoRequest();
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("TiposProduto")
        .WithName("ListarTiposProduto")
        .Produces<ListarTiposProdutoResponse>();
    }
}
