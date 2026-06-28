using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.AtualizarInadimplenciaCliente;

public sealed class AtualizarInadimplenciaClienteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/clientes/{id:guid}/inadimplencia", async (
            Guid id,
            AtualizarInadimplenciaClienteRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("Clientes")
        .WithName("AtualizarInadimplenciaCliente")
        .Produces<AtualizarInadimplenciaClienteResponse>()
        .ProducesValidationProblem();
    }
}
