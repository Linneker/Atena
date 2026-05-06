using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios;

public sealed class ListarUsuariosEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/usuarios", async (
            int? skip,
            int? take,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(
                new ListarUsuariosQuery(skip ?? 0, take ?? 50),
                cancellationToken);
            return response.IsSuccess
                ? Results.Ok(response.Content)
                : Results.Json(response, statusCode: response.Status);
        })
        .RequireAuthorization()
        .WithTags("Usuarios")
        .WithName("ListarUsuarios")
        .Produces<ListarUsuariosQueryResult>();
    }
}
