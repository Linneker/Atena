using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.CriarCliente;

public sealed class CriarClienteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/clientes", async (
            CriarClienteRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/clientes/{response.Id}", response);
        })
        .RequireAuthorization()
        .WithTags("Clientes")
        .WithName("CriarCliente")
        .Produces<CriarClienteResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
