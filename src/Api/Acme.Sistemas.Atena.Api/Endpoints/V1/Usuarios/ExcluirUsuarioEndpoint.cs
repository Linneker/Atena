using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Usuario.Command.ExcluirUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios;

public sealed class ExcluirUsuarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/usuarios/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ExcluirUsuarioCommand(id), cancellationToken);
            return response.IsSuccess
                ? Results.NoContent()
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Usuarios")
        .WithName("ExcluirUsuario")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
