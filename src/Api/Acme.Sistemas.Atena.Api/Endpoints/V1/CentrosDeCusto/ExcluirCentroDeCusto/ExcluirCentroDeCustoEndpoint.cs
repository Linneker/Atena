using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.ExcluirCentroDeCusto;

public sealed class ExcluirCentroDeCustoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/centros-de-custo/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ExcluirCentroDeCustoRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequireAuthorization()
        .WithTags("CentrosDeCusto")
        .WithName("ExcluirCentroDeCusto")
        .Produces(StatusCodes.Status204NoContent);
    }
}
