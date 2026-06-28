using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ConfirmarPedidoVenda;

public sealed class ConfirmarPedidoVendaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/pedidos-venda/{id:guid}/confirmar", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ConfirmarPedidoVendaRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("PedidosVenda")
        .WithName("ConfirmarPedidoVenda")
        .Produces<ConfirmarPedidoVendaResponse>();
    }
}
