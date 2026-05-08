using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.AtribuirRoleAUsuario;

public sealed class AtribuirRoleAUsuarioEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles/{id:guid}/usuarios", async (
            Guid id,
            AtribuirRoleAUsuarioRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Usuario, Permissions.Acoes.Editar))
        .WithTags("Roles")
        .WithName("AtribuirRoleAUsuario")
        .Produces(StatusCodes.Status204NoContent);
    }
}
