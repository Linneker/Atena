using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Despesa.Command.CriarDespesa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.CriarDespesa;
public sealed class CriarDespesaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/despesas", async (
            CriarDespesaCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Created($"/api/v1/despesas/{response.Content!.Id}", response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Despesas")
        .WithName("CriarDespesa")
        .Produces<CriarDespesaCommandResult>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
