using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.AlterarCentroDeCusto;

public sealed class AlterarCentroDeCustoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/centros-de-custo/{id:guid}", async (
            Guid id,
            AlterarCentroDeCustoRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequireAuthorization()
        .WithTags("CentrosDeCusto")
        .WithName("AlterarCentroDeCusto")
        .Produces<AlterarCentroDeCustoResponse>()
        .ProducesValidationProblem();
    }
}
