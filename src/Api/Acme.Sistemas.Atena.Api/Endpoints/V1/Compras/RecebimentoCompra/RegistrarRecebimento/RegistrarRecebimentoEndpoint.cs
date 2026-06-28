using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.RegistrarRecebimento;

public sealed class RegistrarRecebimentoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/recebimentos-compra", async (
            RegistrarRecebimentoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/recebimentos-compra/{response.RecebimentoId}", response);
        })
        .RequireAuthorization()
        .WithTags("RecebimentosCompra")
        .WithName("RegistrarRecebimento")
        .Produces<RegistrarRecebimentoResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
