using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.FecharPeriodo;
public sealed class FecharPeriodoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/fluxo-de-caixa/fechar-periodo", async (
            FecharPeriodoCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Created(
                    $"/api/v1/fluxo-de-caixa/fechamentos/{response.Content!.Id}",
                    response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("FluxoDeCaixa")
        .WithName("FecharPeriodo")
        .Produces<FecharPeriodoCommandResult>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
