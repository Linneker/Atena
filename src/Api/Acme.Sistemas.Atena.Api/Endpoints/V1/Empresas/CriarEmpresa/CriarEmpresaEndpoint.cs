using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.CriarEmpresa;
public sealed class CriarEmpresaEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/empresas", async (
            CriarEmpresaCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Created($"/api/v1/empresas/{response.Content!.Id}", response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Empresas")
        .WithName("CriarEmpresa")
        .Produces<CriarEmpresaCommandResult>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
