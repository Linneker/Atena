using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.ListarRoles;

public sealed class ListarRolesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/roles", async (
            [AsParameters] ListarRolesRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToQuery(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            return Results.Ok(result.Content.ToResponse());
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Role, Permissions.Acoes.Ler))
        .WithTags("Roles")
        .WithName("ListarRoles")
        .Produces<ListarRolesResponse>();
    }
}
