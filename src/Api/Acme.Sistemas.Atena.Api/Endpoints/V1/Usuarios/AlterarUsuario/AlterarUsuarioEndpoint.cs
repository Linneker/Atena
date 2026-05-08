using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Services.V1.Usuario.Command.AlterarUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.AlterarUsuario;
public sealed record AlterarUsuarioRequest(string NomeCompleto, string Email, StatusAtivo Status);

public sealed class AlterarUsuarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/usuarios/{id:guid}", async (
            Guid id,
            AlterarUsuarioRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new AlterarUsuarioCommand(id, request.NomeCompleto, request.Email, request.Status);
            var response = await mediator.Send(command, cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Usuarios")
        .WithName("AlterarUsuario")
        .Produces<AlterarUsuarioCommandResult>()
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
