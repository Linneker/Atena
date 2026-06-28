using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.CriarPedidoVenda;

public sealed class CriarPedidoVendaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/pedidos-venda", async (
            CriarPedidoVendaRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/pedidos-venda/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("PedidosVenda")
        .WithName("CriarPedidoVenda")
        .Produces<CriarPedidoVendaResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
