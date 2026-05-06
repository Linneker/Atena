using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios;

public sealed class ObterUsuarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/usuarios/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ObterUsuarioQuery(id), cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Usuarios")
        .WithName("ObterUsuario")
        .Produces<ObterUsuarioQueryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
