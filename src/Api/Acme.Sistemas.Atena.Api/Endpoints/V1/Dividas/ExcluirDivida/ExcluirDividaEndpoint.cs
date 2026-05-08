using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.ExcluirDivida;

public sealed class ExcluirDividaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/dividas/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var request = new ExcluirDividaRequest(id);
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequireAuthorization()
        .WithTags("Dividas")
        .WithName("ExcluirDivida")
        .Produces(StatusCodes.Status204NoContent);
    }
}
