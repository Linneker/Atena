using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Usuario.Command.CriarUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios;

public sealed class CriarUsuarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/usuarios", async (
            CriarUsuarioCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Created($"/api/v1/usuarios/{response.Content!.Id}", response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Usuarios")
        .WithName("CriarUsuario")
        .Produces<CriarUsuarioCommandResult>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
