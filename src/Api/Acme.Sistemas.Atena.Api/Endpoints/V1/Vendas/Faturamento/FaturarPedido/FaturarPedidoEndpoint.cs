using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.FaturarPedido;

public sealed class FaturarPedidoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/faturamentos", async (
            FaturarPedidoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/faturamentos/{response.FaturamentoId}", response);
        })
        .RequireAuthorization()
        .WithTags("Faturamentos")
        .WithName("FaturarPedido")
        .Produces<FaturarPedidoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
