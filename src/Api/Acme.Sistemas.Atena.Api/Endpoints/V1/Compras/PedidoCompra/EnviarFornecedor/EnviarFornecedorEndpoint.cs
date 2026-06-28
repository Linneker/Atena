using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.EnviarFornecedor;

public sealed class EnviarFornecedorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/pedidos-compra/{id:guid}/enviar-fornecedor", async (
            Guid id,
            EnviarFornecedorRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("PedidosCompra")
        .WithName("EnviarFornecedor")
        .Produces<EnviarFornecedorResponse>()
        .ProducesValidationProblem();
    }
}
