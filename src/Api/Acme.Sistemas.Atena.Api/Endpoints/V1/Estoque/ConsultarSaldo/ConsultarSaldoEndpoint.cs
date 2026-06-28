using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.ConsultarSaldo;

public sealed class ConsultarSaldoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/estoque/produtos/{produtoId:guid}/saldo", async (
            Guid produtoId,
            Guid? estoqueId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ConsultarSaldoRequest(produtoId, estoqueId);
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Estoque")
        .WithName("ConsultarSaldo")
        .Produces<ConsultarSaldoResponse>();
    }
}
