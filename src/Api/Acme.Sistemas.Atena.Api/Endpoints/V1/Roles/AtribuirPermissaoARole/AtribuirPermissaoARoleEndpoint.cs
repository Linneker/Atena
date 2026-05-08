using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.AtribuirPermissaoARole;

public sealed class AtribuirPermissaoARoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles/{id:guid}/permissoes", async (
            Guid id,
            AtribuirPermissaoARoleRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(id), cancellationToken);
            return result.IsSuccess
                ? Results.NoContent()
                : Results.Json(result, statusCode: result.Status);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Role, Permissions.Acoes.Editar))
        .WithTags("Roles")
        .WithName("AtribuirPermissaoARole")
        .Produces(StatusCodes.Status204NoContent);
    }
}
