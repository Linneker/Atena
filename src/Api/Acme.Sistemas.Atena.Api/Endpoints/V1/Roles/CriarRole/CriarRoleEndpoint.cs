using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Mediators;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Roles.CriarRole;

public sealed class CriarRoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/roles", async (
            CriarRoleRequest request,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            if (!result.IsSuccess || result.Content is null)
                return Results.Json(result, statusCode: result.Status);

            var response = result.Content.ToResponse();
            return Results.Created($"/api/v1/roles/{response.Id}", response);
        })
        .RequirePermissao(Permissions.Of(Permissions.Recursos.Role, Permissions.Acoes.Criar))
        .WithTags("Roles")
        .WithName("CriarRole")
        .Produces<CriarRoleResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem();
    }
}
