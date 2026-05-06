using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Receita.Command.CriarReceita;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita;

public sealed class CriarReceitaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/receitas", async (
            CriarReceitaCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Created($"/api/v1/receitas/{response.Content!.Id}", response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Receitas")
        .WithName("CriarReceita")
        .Produces<CriarReceitaCommandResult>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
